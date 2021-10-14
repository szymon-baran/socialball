import axios from "axios";
import { getField, updateField } from "vuex-map-fields";

export default {
  namespaced: true,
  state() {
    return {
      players: [],
      player: {
        FirstName: "",
        LastName: "",
        Email: "",
        Position: null,
        TeamId: "",
        Citizenship: "",
        LoginUsername: "",
        LoginPassword: "",
      },
    };
  },
  getters: {
    getPlayers(state) {
      return state.players;
    },
    getPlayerDetails(state) {
      return state.playerDetails;
    },
    getField,
  },
  mutations: {
    SET_PLAYERS(state, payload) {
      state.players = payload;
    },
    RESET_PLAYERS(state) {
      state.players = [];
    },
    SET_PLAYER_DETAILS(state, payload) {
      state.player.FirstName = payload.firstName;
      state.player.LastName = payload.lastName;
      state.player.Email = payload.email;
      state.player.Position = payload.position;
      state.player.TeamId = payload.teamId;
      state.player.Citizenship = payload.citizenship;
    },
    RESET_PLAYER_FORM(state) {
      state.player.FirstName = "";
      state.player.LastName = "";
      state.player.Email = "";
      state.player.Position = null;
      state.player.TeamId = "";
      state.player.Citizenship = "";
      state.player.LoginUsername = "";
      state.player.LoginPassword = "";
    },
    updateField,
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
    addPlayer: async ({ state }) => {
      await axios.post("https://localhost:44369/api/players", state.player);
    },
    setPlayerDetails: async ({ commit }, playerId) => {
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
