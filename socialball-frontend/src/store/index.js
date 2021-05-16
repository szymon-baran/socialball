import { createStore } from "vuex";

import playersModule from "./modules/players/index";
import teamsModule from "./modules/teams/index";

const store = createStore({
  modules: {
    players: playersModule,
    teams: teamsModule,
  },
});

export default store;
