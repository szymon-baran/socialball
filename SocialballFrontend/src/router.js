import { createRouter, createWebHistory } from 'vue-router';

//Authentication
import LoginPlayer from './pages/authentication/LoginPlayer.vue';
import UserProfile from './pages/authentication/UserProfile.vue';

//Players
import PlayerDetails from './pages/players/PlayerDetails.vue';
import PlayerRegistration from './pages/players/PlayerRegistration.vue';
import PlayersList from './pages/players/PlayersList.vue';

//Teams
import TeamDetails from './pages/teams/TeamDetails.vue';
import TeamsList from './pages/teams/TeamsList.vue';

//Matches
import MatchAdd from './pages/matches/MatchAdd.vue';
import MatchDetails from './pages/matches/MatchDetails.vue';
import MatchesList from './pages/matches/MatchesList.vue';

//Messages
import ContactPlayer from './pages/messages/ContactPlayer.vue';
import ContactTeam from './pages/messages/ContactTeam.vue';
import MessagesReceived from './pages/messages/MessagesReceived.vue';

//Others
import HomePage from './pages/HomePage.vue';
import PageNotFound from './pages/PageNotFound.vue';


const router = createRouter({
    history: createWebHistory(),
    routes: [
        { path: '/', component: HomePage },
        // { path: '/', redirect: '/home' },
        { path: '/profile', component: UserProfile }, // profil zalogowanego uzytkownika
        { path: '/players', component: PlayersList },
        { path: '/players/:id', component: PlayerDetails, children: [
            { path: 'contact', component: ContactPlayer }, // /players/:id/contact
        ] },
        { path: '/register', component: PlayerRegistration },
        { path: '/teams', component: TeamsList },
        { path: '/teams/:id', component: TeamDetails, children: [
            { path: 'contact', component: ContactTeam },
        ] },
        { path: '/addMatch', component: MatchAdd },
        { path: '/matches', component: MatchesList },  
        { path: '/matches/:id', component: MatchDetails },
        { path: '/login', component: LoginPlayer },
        { path: '/messages', component: MessagesReceived },
        { path: '/:pageNotFound(.*)', component: PageNotFound },
    ],
});

export default router;