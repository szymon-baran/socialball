import axios from "axios";

export default {
  namespaced: true,
  state() {
    return {
      teams: [],
      positionsData: [],
    };
  },
  getters: {
    getTeams(state) {
      return state.teams;
    },
    getPositionsData(state) {
      return state.positionsData;
    },
  },
  mutations: {
    SET_TEAMS(state, payload) {
      state.teams = payload;
    },
    RESET_TEAMS(state) {
      state.teams = [];
    },
    SET_POSITIONS_DATA(state, payload) {
      state.positionsData = payload;
    },
  },
  actions: {
    setAllTeams({ commit }) {
      axios.get("https://localhost:44369/api/teams").then((response) => {
        commit("SET_TEAMS", response.data);
      });
    },
    setPositionsInsideOfTeam: async ({ commit }, teamId) => {
      await axios
        .get("https://localhost:44369/api/teams/getPositionsInsideOfTeam", {
          params: { teamId: teamId },
        })
        .then(function(response) {
          commit("SET_POSITIONS_DATA", response.data);
        });
    },
    setPositionsToLookup: () => {
      return new Promise((resolve, reject) =>
        axios
          .get("https://localhost:44369/api/teams/getPositionsToLookup")
          .then(
            (response) => {
              resolve(response);
            },
            (error) => {
              reject(error);
            }
          )
      );
    },
    setLeaguesToLookup: () => {
      return new Promise((resolve, reject) =>
        axios
          .get("https://localhost:44369/api/teams/getLeaguesToLookup")
          .then(
            (response) => {
              resolve(response);
            },
            (error) => {
              reject(error);
            }
          )
      );
    },
  },
};
