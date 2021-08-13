import { NgModule, APP_INITIALIZER, Optional, SkipSelf } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { AuthService } from "./services/auth.service";
import { JwtInterceptor } from "./interceptors/jwt.interceptor";
import { UnauthorizedInterceptor } from "./interceptors/unauthorized.interceptor";
import { appInitializer } from "./services/app-initializer";

@NgModule({
	declarations: [],
	imports: [CommonModule],
	providers: [
    {
			provide: APP_INITIALIZER,
			useFactory: appInitializer,
			multi: true,
			deps: [AuthService],
		},
		{ provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
		{
			provide: HTTP_INTERCEPTORS,
			useClass: UnauthorizedInterceptor,
			multi: true,
		},
	],
})
export class CoreModule {
	constructor(@Optional() @SkipSelf() core: CoreModule) {
		if (core) {
			throw new Error("Core Module can only be imported to AppModule.");
		}
	}
}
