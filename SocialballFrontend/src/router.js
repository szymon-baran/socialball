import { createRouter, createWebHistory } from "vue-router";
import store from "./store/index";

//Authentication
import UserProfile from "./pages/authentication/UserProfile.vue";

//Players
import PlayerDetails from "./pages/players/PlayerDetails.vue";
import RegisterEditPlayer from "./pages/players/RegisterEditPlayer.vue";
import PlayersList from "./pages/players/PlayersList.vue";
import InjuriesList from "./pages/players/InjuriesList.vue";

//Teams
import TeamDetails from "./pages/teams/TeamDetails.vue";
import TeamsList from "./pages/teams/TeamsList.vue";
import TeamAdd from "./pages/teams/TeamAdd.vue";
import AdminTeamsList from "./pages/admin/AdminTeamsList.vue";

//Matches
import MatchesList from "./pages/matches/MatchesList.vue";
import UnconfirmedMatchesList from "./pages/matches/UnconfirmedMatchesList.vue";

//Messages
import MessagesList from "./pages/messages/MessagesList.vue";

//Job advertisements
import JobAdvertisements from "./pages/jobAdvertisements/JobAdvertisements.vue";

//Transfer offers
import PlayerTransferOffers from "./pages/playerTransferOffers/PlayerTransferOffers.vue";

//Others
import HomePage from "./pages/HomePage.vue";
import PageNotFound from "./pages/PageNotFound.vue";
import NoPermissionPage from "./pages/NoPermissionPage.vue";
import AdminDashboard from "./pages/admin/AdminDashboard.vue";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: "/", component: HomePage, meta: { guest: true } },
    { path: "/profile", component: UserProfile, meta: { guest: false } },
    { path: "/players", component: PlayersList, meta: { admin: true } },
    {
      path: "/players/:id",
      component: PlayerDetails,
      name: "playerDetails",
      meta: { admin: true }
    },
    { path: "/register", component: RegisterEditPlayer, meta: { guest: true } },
    {
      path: "/profile-edit",
      name: "editProfile",
      component: RegisterEditPlayer,
      params: true,
      meta: { guest: false },
    },
    { path: "/teams", component: TeamsList, meta: { admin: true } },
    { path: "/admin-teams", component: AdminTeamsList, meta: { admin: true, adminOnly: true } },
    {
      path: "/teams/:id",
      component: TeamDetails,
      params: true,
      name: "teamDetails",
      meta: { admin: true }
    },
    { path: "/add-team", component: TeamAdd, meta: { guest: true } },
    { path: "/matches", component: MatchesList, meta: { admin: true } },
    {
      path: "/unconfirmed-matches",
      component: UnconfirmedMatchesList,
      meta: { guest: false, player: false },
    },
    { path: "/messages", component: MessagesList, meta: { guest: false } },
    {
      path: "/job-advertisements",
      component: JobAdvertisements,
      meta: { guest: false },
    },
    {
      path: "/team-management",
      component: InjuriesList,
      meta: { guest: false },
    },
    {
      path: "/transfer-offers",
      component: PlayerTransferOffers,
      meta: { guest: false, player: false },
    },
    { path: "/:pageNotFound(.*)", component: PageNotFound },
    { path: "/no-permission", component: NoPermissionPage },
    { path: "/admin", component: AdminDashboard, meta: { admin: true, adminOnly: true } },
  ],
});

router.beforeEach((to, from, next) => {
  const isAuthenticated = store.getters["authentication/isLoggedIn"];
  const getUserType = store.getters["authentication/getLoggedInUserType"];
  if (to.meta.guest != null && !to.meta.guest) {
    if (!isAuthenticated) {
      return next({ path: "/no-permission" });
    }
  }

  if (
    to.meta.guest != null &&
    !to.meta.guest &&
    to.meta.player != null &&
    !to.meta.player
  ) {
    if (getUserType != 2) {
      return next({ path: "/no-permission" });
    }
  }

  if (to.meta.adminOnly) {
    if (getUserType != 10) {
      return next({ path: "/no-permission" });
    }
  }

  if (to.meta.guest != null && to.meta.guest) {
    if (isAuthenticated) {
      return next({ path: "/profile" });
    }
  }

  if (getUserType == 10 && (to.meta.admin == null || to.meta.admin == false)) {
    return next({ path: "/admin" });
  }
  return next();
});

export default router;
