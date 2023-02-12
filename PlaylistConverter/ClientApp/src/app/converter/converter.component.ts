import { Component, Inject } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { AfterViewInit, ElementRef } from '@angular/core';

declare let require: any;
declare let MusicKit: any;
@Component({
  selector: 'app-converter',
  templateUrl: './converter.component.html',
  styleUrls: ['./converter.component.css']
})
export class ConverterComponent {

  playlist: any;
  linkControl = new FormControl('');
  default = "https://open.spotify.com/playlist/5aJjvZYiPfszshEDxBcgpZ?si=23064bf926b149ab"
  music: any;
  token: string = '';
  appleSongs: any[] = [];
  creationSuccesful: any = false;

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string
  ) {
    this.getAppleDevToken();
  };

  async ConfigureMusicKit() {
    console.log("made it!")
    let error = false;
    console.log(this.token);
    try {
      await MusicKit.configure({
        developerToken: this.token,
        app: {
          name: 'PlaylistConverter',
          build: '1978.4.1',
        },
      });
    } catch (err) {
      error = true;
      console.log("got an error")
      // Handle configuration error
    }

    if (!error) {
      // MusicKit instance is available
      this.music = MusicKit.getInstance();
      this.testApple();
    }
    
  }

  async testApple(): Promise<void>  {
    const headers = { 'Authorization': 'Bearer ' + this.token }
    let something = await this.http.get<any>('https://api.music.apple.com/v1/test', { headers }).toPromise();
    console.log(something);
  }

  async searchSong(): Promise<void>  {
    const headers = { 'Authorization': 'Bearer ' + this.token }
    for (const song of this.playlist.songs) {
      try {
        let result = await this.http.get<any>('https://api.music.apple.com/v1/catalog/us/search?types=songs,albums,artists&limit=1' + '&term=' + song.title + '+' + song.artists[0], { headers }).toPromise();
        if (result.results.songs != undefined)
          this.appleSongs.push(result.results.songs.data[0])

        console.log(result);
      }
      catch (ex) {  }
    }
  }

  getAppleDevToken(): void {
    this.http.get<any>(this.baseUrl + 'playlist').subscribe(result => {
      this.token = result;
      this.ConfigureMusicKit();
    }, error => console.error(error));
  }

  async CreatePlaylist(): Promise<void>  {
    const headers = { 'Authorization': 'Bearer ' + this.token, 'Music-User-Token': this.music.api.userToken }
    let result = await this.http.get<any>('https://api.music.apple.com/v1/me/library/playlist-folders?filter[identity]=playlistsroot', { headers }).toPromise();
    console.log(result);
    const data = [];
    for (const song of this.appleSongs) {
      let temp = { id: song.id, type: 'songs' }
      data.push(temp);
    }

    const tracks = { data: data }
    const parentData = [{id : result.meta.filters.identity.playlistsroot[0].id, type : 'library-playlist-folders'}]
    const parent = {data: parentData}
    const attributes = { description: this.playlist.description, name: this.playlist.name }
    const relationships = { tracks: tracks, parent }
    const dataToSend = {attributes : attributes, relationships : relationships}
    let playlist = await this.http.post<any>('https://api.music.apple.com/v1/me/library/playlists', JSON.stringify(dataToSend), { headers }).toPromise();
    console.log(playlist);
    this.creationSuccesful = true;
  }

  async onSubmit(): Promise<void> {
    this.appleSongs = [];
    //this.playlist.songs = [];
    let linkValue = this.linkControl.value;

    const regex = '[0-9A-Za-z]+\\?';

    let found = linkValue.match(regex);

    let playlistId = found[0].slice(0, -1);

    let data = { Id: playlistId };

    const headers = { 'content-type': 'application/json' };
    this.playlist = await this.http.post<any>(this.baseUrl + 'playlist', JSON.stringify(data), { headers }).toPromise();
    console.log(this.playlist);
    await this.searchSong();
    await this.music.authorize();
    this.CreatePlaylist();
    
  }
}
