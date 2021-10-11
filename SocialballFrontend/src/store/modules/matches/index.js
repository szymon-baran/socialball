import axios from "axios";
import { getField, updateField } from "vuex-map-fields";

export default {
  namespaced: true,
  state() {
    return {
      matches: [],
      match: {
        Id: "",
        HomeTeamId: "",
        HomeTeam: {},
        AwayTeamId: "",
        AwayTeam: {},
        Stadium: "",
        DateTime: null,
        Goals: [],
      },
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
    SET_MATCH_DETAILS(state, payload) {
      state.match.Id = payload.id;
      state.match.HomeTeamId = payload.homeTeamId;
      state.match.HomeTeam = payload.homeTeam;
      state.match.AwayTeamId = payload.awayTeamId;
      state.match.AwayTeam = payload.awayTeam;
      state.match.Stadium = payload.stadium;
      state.match.DateTime = payload.dateTime;
      state.match.Goals = payload.goals;
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
    setMatchDetails: async ({ commit }, matchId) => {
      await axios
        .get("https://localhost:44369/api/matches/details", {
          params: { id: matchId },
        })
        .then(function(response) {
          commit("SET_MATCH_DETAILS", response.data);
        });
    },
  },
};
