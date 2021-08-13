import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, map } from "rxjs/operators";
import { BehaviorSubject, Observable, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private userSubject: BehaviorSubject<User>;
	public user: Observable<User>;
	private readonly apiUrl = `${environment.apiUrl}user`;

	private isAuthenticatedSubject = new ReplaySubject<boolean>(1);
	public isAuthenticated = this.isAuthenticatedSubject.asObservable();

	constructor(private router: Router, private http: HttpClient) {
		this.userSubject = new BehaviorSubject<User>(null);
		this.user = this.userSubject.asObservable();
	}

	public get userValue(): User {
		return this.userSubject.value;
	}

	login(email: string, password: string, rememberMe: boolean) {
		return this.http
			.post<any>(
				`${this.apiUrl}/login`,
				{ email, password, rememberMe },
				{ withCredentials: true }
			)
			.pipe(
				map((user) => {
					this.userSubject.next(user);
          localStorage.setItem('currentUser', JSON.stringify(user));
					this.isAuthenticatedSubject.next(true);
					return user;
				})
			);
	}

	register(username: string, email: string, password: string) {
		return this.http
			.post<any>(
				`${this.apiUrl}/register`,
				{ username, email, password },
				{ withCredentials: true }
			)
			.pipe(
				map((user) => console.log(user)),
				catchError((error) => {
					console.log(error);
					return error;
				})
			);
	}

	logout() {
		// this.http
		// 	.post<any>(`${this.apiUrl}/logout`, {}, { withCredentials: true })
		// 	.subscribe();
		this.isAuthenticatedSubject.next(false);
    localStorage.removeItem('currentUser');
		this.userSubject.next(null);
	}

  GetUserFromStorage(){
    let user = JSON.parse(localStorage.getItem("currentUser"));
    this.userSubject.next(user);
    this.isAuthenticatedSubject.next(true);

  }
}
