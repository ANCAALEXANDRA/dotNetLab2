import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { MoviesService } from '../movies.service';
import { Movie } from '../movies.model';

@Component({
  selector: 'app-list-movies',
  templateUrl: './movies-list.component.html',
  styleUrls: ['./movies-list.component.css']
})
export class MoviesListComponent{

  public movies: Movie[];

  constructor(private moviesService: MoviesService) {
   
  }

  getMovies() {
    this.moviesService.getMovies().subscribe(m => this.movies = m)
  }

  ngOnInit() {
    this.getMovies();
  }

}
