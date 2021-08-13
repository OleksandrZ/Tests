import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../core/services/auth.service';
import { User } from '../core/models/user.model';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  currentUser: User;
  constructor(
    private router: Router,
    private authenticationService: AuthService
) {
    this.authenticationService.user.subscribe(x => this.currentUser = x);
}
  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout() {
    this.authenticationService.logout();
    this.router.navigate(['/login']);
  }
}
