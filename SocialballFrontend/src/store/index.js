import { createStore } from "vuex";

import playersModule from "./modules/players/index";
import teamsModule from "./modules/teams/index";
import matchesModule from "./modules/matches/index";
import authenticationModule from "./modules/authentication/index";

const store = createStore({
  modules: {
    players: playersModule,
    teams: teamsModule,
    matches: matchesModule,
    authentication: authenticationModule,
  },
});

export default store;
