export class Movie {
  Id: number;
  Title: string;
  Description: string;
  Gender: string;
  DurationInMinutes: number;
  YearOfRelease:string;
  Director: string;
  DateAdded: Date;
  Rating: number;
  Watched: boolean;

}

export class PaginatedMovies {
  firstPages: number[];
  lastPages: number[];
  previousPages: number[];
  nextPages: number[];
  totalEntities: number;
  entities: Movie[];
}
