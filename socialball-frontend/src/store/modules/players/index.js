import axios from "axios";

export default {
  namespaced: true,
  state() {
    return {
      players: [],
    };
  },
  getters: {
    getPlayers(state) {
      debugger;
      return state.players;
    },
  },
  mutations: {
    SET_PLAYERS(state, payload) {
      state.players = payload;
    },
  },
  actions: {
    getAllPlayers({ commit }) {
      axios
        .get("https://localhost:44369/api/players")
        .then((response) => {
          commit("SET_PLAYERS", response.data);
        });
    },
  },
};
