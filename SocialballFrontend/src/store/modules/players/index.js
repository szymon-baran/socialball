import axios from "axios";

export default {
  namespaced: true,
  state() {
    return {
      form: {
        FirstName: "",
        LastName: "",
        Team: "",
        Email: "",
      },
      players: [],
      playerDetails: [],
    };
  },
  getters: {
    getPlayers(state) {
      return state.players;
    },
    getPlayerDetails(state) {
      return state.playerDetails;
    },
  },
  mutations: {
    SET_PLAYERS(state, payload) {
      state.players = payload;
    },
    SET_PLAYER_DETAILS(state, payload) {
      state.playerDetails = payload;
    },
  },
  actions: {
    setPlayers({ commit }, profileTeamId) {
      axios
        .get("https://localhost:44369/api/players", {
          params: { teamId: profileTeamId },
        })
        .then((response) => {
          commit("SET_PLAYERS", response.data);
        });
    },
    addPlayer: async ({ commit }, player) => {
      console.log(commit);
      await axios
        .post("https://localhost:44369/api/players", player);
    },
    setPlayerDetails: async ({ commit }, playerId) => {
      console.log(commit);
      await axios
        .get("https://localhost:44369/api/players/details", {
          params: { id: playerId },
        })
        .then(function(response) {
          commit("SET_PLAYER_DETAILS", response.data);
        });
    },
  },
};
