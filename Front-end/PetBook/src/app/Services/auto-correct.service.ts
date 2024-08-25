import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

interface Breed {
  breedID: number;
  breed1: string;
}

@Injectable({
  providedIn: 'root'
})
export class AutoCorrectService {

  breeds: string[] = [];

  private url = 'https://localhost:7066/api/Breed';

  constructor(private http: HttpClient) {
    // this.fetchBreeds();
    this.http.get<Breed[]>(this.url)
    .pipe(
      map((data: Breed[]) => data.map(breed => breed.breed1))
    )
    .subscribe((names: string[]) => {
      this.breeds = names;
    });
  }

  getBreedSuggestions(input: string): string[] {
    console.log("from service",this.breeds.filter(breed => breed.toLowerCase().includes(input.toLowerCase())))
    return this.breeds.filter(breed => breed.toLowerCase().includes(input.toLowerCase()));
  }
}
