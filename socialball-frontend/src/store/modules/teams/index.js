import axios from "axios";

export default {
  namespaced: true,
  state() {
    return {
      teams: [],
    };
  },
  getters: {
    getTeams(state) {
      debugger;
      return state.teams;
    },
  },
  mutations: {
    SET_TEAMS(state, payload) {
      state.teams = payload;
    },
  },
  actions: {
    getAllTeams({ commit }) {
      axios
        .get("https://localhost:44369/api/teams")
        .then((response) => {
          commit("SET_TEAMS", response.data);
        });
    },
  },
};
