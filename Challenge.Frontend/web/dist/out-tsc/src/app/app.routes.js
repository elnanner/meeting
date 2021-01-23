import { RouterModule } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { MeetingComponent } from './components/meeting/meeting.component';
const APP_ROUTES = [
    { path: 'login', component: LoginComponent },
    { path: 'home', component: HomeComponent },
    { path: 'meetings', component: MeetingComponent },
    { path: '**', component: LoginComponent }
];
export const APP_ROUTING = RouterModule.forRoot(APP_ROUTES);
//# sourceMappingURL=app.routes.js.map