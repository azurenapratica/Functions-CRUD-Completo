import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Person } from '../person';
import { environment } from '../../environments/environment'
import { take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PeopleService {

  private readonly _API = `${environment.API}`;
  private readonly _KEY = `${environment.KEY}`;
  private readonly _headers = new HttpHeaders({ "Ocp-Apim-Subscription-Key": this._KEY });

  constructor(private http: HttpClient) { }

  list() {
    return this.http.get<Person[]>(this._API + 'ListPeople', { headers: this._headers });
  }

  getById(id) {
    return this.http.get<Person>(`${this._API}GetPerson?id=${id}`, { headers: this._headers })
      .pipe(take(1));
  }

  private create(person) {
    return this.http.post(this._API + 'CreatePerson', JSON.stringify(person), { headers: this._headers, responseType: 'text' })
      .pipe(take(1));
  }

  private update(person) {
    return this.http.put(this._API + 'EditPerson', JSON.stringify(person), { headers: this._headers, responseType: 'text' })
      .pipe(take(1));
  }

  save(person) {
    if (person.rowKey)
      return this.update(person);
    else
      return this.create(person);
  }

  remove(id) {
    return this.http.delete(`${this._API}DeletePerson?id=${id}`, { headers: this._headers, responseType: 'text' })
      .pipe(take(1));
  }
}
