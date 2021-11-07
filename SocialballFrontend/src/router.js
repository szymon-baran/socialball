import { createRouter, createWebHistory } from "vue-router";
import store from "./store/index";

//Authentication
import UserProfile from "./pages/authentication/UserProfile.vue";

//Players
import PlayerDetails from "./pages/players/PlayerDetails.vue";
import PlayerRegistration from "./pages/players/PlayerRegistration.vue";
import PlayersList from "./pages/players/PlayersList.vue";

//Teams
import TeamDetails from "./pages/teams/TeamDetails.vue";
import TeamsList from "./pages/teams/TeamsList.vue";

//Matches
import MatchAdd from "./pages/matches/MatchAdd.vue";
import MatchesList from "./pages/matches/MatchesList.vue";

//Messages
import MessagesList from "./pages/messages/MessagesList.vue";

//Others
import HomePage from "./pages/HomePage.vue";
import PageNotFound from "./pages/PageNotFound.vue";
import NoPermissionPage from "./pages/NoPermissionPage.vue";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: "/", component: HomePage },
    // { path: '/', redirect: '/home' },
    { path: "/profile", component: UserProfile, meta: { guest: false } }, // profil zalogowanego uzytkownika
    { path: "/players", component: PlayersList },
    {
      path: "/players/:id",
      component: PlayerDetails,
      // children: [
      //   { path: "contact", component: ContactPlayer }, // /players/:id/contact
      // ],
    },
    { path: "/register", component: PlayerRegistration },
    { path: "/teams", component: TeamsList },
    {
      path: "/teams/:id",
      component: TeamDetails,
      params: true,
      // children: [{ path: "contact", component: ContactTeam }],
    },
    { path: "/addMatch", component: MatchAdd },
    { path: "/matches", component: MatchesList },
    { path: "/messages", component: MessagesList, meta: { guest: false }  },
    { path: "/:pageNotFound(.*)", component: PageNotFound },
    { path: "/no-permission", component: NoPermissionPage }
  ],
});

router.beforeEach((to, from, next) => {
  if (to.meta.guest != null && !to.meta.guest) {
    const isAuthenticated = store.getters["authentication/isLoggedIn"];
    if (!isAuthenticated) {
      return next({ path: "/no-permission" });
    }
  }
  return next();
});

export default router;
