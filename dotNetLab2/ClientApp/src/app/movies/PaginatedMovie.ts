import { Movie } from "./movies.model";

export class PaginatedMovies {
  firstPages: number[];
  lastPages: number[];
  previousPages: number[];
  nextPages: number[];
  totalEntities: number;
  entities: Movie[];
}
