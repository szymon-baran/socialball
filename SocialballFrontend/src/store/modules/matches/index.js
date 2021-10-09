import axios from "axios";
import { getField, updateField } from "vuex-map-fields";

export default {
  namespaced: true,
  state() {
    return {
      matches: [],
      match: {},
    };
  },
  getters: {
    getMatches(state) {
      return state.matches;
    },
    // getGoalsInMatches(state) {
    //   let tmp = [];
    //   state.matches.forEach((entry) => {
    //     entry.goals.forEach((x) => {
    //       tmp.push(x);
    //     })
    //   });
    //   return tmp;
    // },
    getField,
  },
  mutations: {
    SET_MATCHES(state, payload) {
      state.matches = payload;
      state.matches.forEach((match) => {
        let homeGoals = 0;
        let awayGoals = 0;
        match.goals.forEach((goal) => {
          if (goal.scorer.teamId === match.homeTeamId) {
            homeGoals++;
          } else if (goal.scorer.teamId === match.awayTeamId) {
            awayGoals++;
          }
        });
        match.result = homeGoals + ":" + awayGoals;
      });
    },
    RESET_MATCHES(state) {
      state.matches = [];
    },
    updateField,
  },

  actions: {
    setMatches({ commit }, teamId) {
      axios
        .get("https://localhost:44369/api/matches", {
          params: { teamId: teamId },
        })
        .then((response) => {
          commit("SET_MATCHES", response.data);
        });
    },
  },
};
