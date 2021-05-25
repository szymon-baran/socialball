import { createStore } from "vuex";

import playersModule from "./modules/players/index";
import teamsModule from "./modules/teams/index";

const store = createStore({
  state() {
    return {
      user: {
        id: "5dcb2ff6-8e31-4cd0-b619-685d0590524a",
        firstName: "Robert",
        lastName: "Lewandowski",
        position: 3,
        teamId: "36470F04-6238-4E72-B157-DABE95220A54"
      }
    };
  },
  getters: {
    getCurrentUser: state => {
      return state.user;
    },
    getUserFullname: state => {
      return state.user.firstName + " " + state.user.lastName;
    },
  },
  modules: {
    players: playersModule,
    teams: teamsModule,
  },
});

export default store;
