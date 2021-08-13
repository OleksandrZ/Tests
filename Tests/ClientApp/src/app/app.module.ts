import { BrowserModule } from "@angular/platform-browser";
import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";
import { RouterModule } from "@angular/router";
import { AppComponent } from "./app.component";
import { NavMenuComponent } from "./nav-menu/nav-menu.component";
import { HomeComponent } from "./home/home.component";
import { AuthComponent } from "./auth/auth.component";
import { CoreModule } from "./core/core.module";
import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { AuthGuard } from "./core/guards/auth.guard";
import { CreateTestComponent } from "./create-test/create-test.component";
import { TestPageComponent } from "./test-page/test-page.component";
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    AuthComponent,
    CreateTestComponent,
    TestPageComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: "ng-cli-universal" }),
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    CoreModule,
    CommonModule,
    RouterModule.forRoot(
      [
        {
          path: "",
          component: HomeComponent,
          pathMatch: "full",
          canActivate: [AuthGuard],
        },
        { path: "login", component: AuthComponent },
        { path: "register", component: AuthComponent },
        {
          path: "test/:id",
          component: TestPageComponent,
          canActivate: [AuthGuard],
        },
        {
          path: "create-test",
          component: CreateTestComponent,
          // canActivate: [AuthGuard],
        },

      ],
      { relativeLinkResolution: "legacy" }
    ),
    FontAwesomeModule,
    NgbModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
