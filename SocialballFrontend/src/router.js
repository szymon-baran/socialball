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

//Job advertisements
import JobAdvertisements from "./pages/jobAdvertisements/JobAdvertisements.vue";

//Others
import HomePage from "./pages/HomePage.vue";
import PageNotFound from "./pages/PageNotFound.vue";
import NoPermissionPage from "./pages/NoPermissionPage.vue";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: "/", component: HomePage, meta: { guest: true } },
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
    { path: "/register", component: PlayerRegistration, meta: { guest: true } },
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
    { path: "/job-advertisements", component: JobAdvertisements, meta: { guest: false }  },
    { path: "/:pageNotFound(.*)", component: PageNotFound },
    { path: "/no-permission", component: NoPermissionPage }
  ],
});

router.beforeEach((to, from, next) => {
  const isAuthenticated = store.getters["authentication/isLoggedIn"];
  if (to.meta.guest != null && !to.meta.guest) {
    if (!isAuthenticated) {
      return next({ path: "/no-permission" });
    }
  }

  if (to.meta.guest != null && to.meta.guest) {
    if (isAuthenticated) {
      return next({ path: "/profile" });
    }
  }
  return next();
});

export default router;
