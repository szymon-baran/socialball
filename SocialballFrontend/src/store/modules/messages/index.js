import axios from "axios";
import { getField, updateField } from "vuex-map-fields";
import authHeader from "../../../services/auth-header";

export default {
  namespaced: true,
  state() {
    return {
      messages: [],
      teamMessage: {
        FromUserId: "",
        ToTeamId: "",
        Subject: "",
        Content: "",
        SentOn: null,
      },
    };
  },
  getters: {
    getMessages(state) {
      return state.messages;
    },
    getTeamMessageDetails(state) {
      return state.teamMessage;
    },
    getField,
  },
  mutations: {
    SET_MESSAGES(state, payload) {
      state.messages = payload;
    },
    RESET_MESSAGES(state) {
      state.messages = [];
    },
    SET_TEAM_MESSAGE_DETAILS(state, payload) {
      state.player.FromUserId = payload.fromUserId;
      state.player.ToTeamId = payload.toTeamId;
      state.player.Subject = payload.subject;
      state.player.Content = payload.content;
      state.player.SentOn = payload.sentOn;
    },
    RESET_TEAM_MESSAGE_FORM(state) {
      state.player.FromUserId = "";
      state.player.ToTeamId = "";
      state.player.Subject = "";
      state.player.Content = "";
      state.player.SentOn = null;
    },
    updateField,
  },

  actions: {
    setMessages({ commit }, userId) {
      axios
        .get("https://localhost:44369/api/messages", {
          params: { userId: userId },
          headers: authHeader(),
        })
        .then((response) => {
          commit("SET_MESSAGES", response.data);
        });
    },
    sendTeamMessage: async ({ state, dispatch }) => {
      await axios
        .post(
          "https://localhost:44369/api/messages/addTeamMessage",
          state.teamMessage
        )
        .then(() => {
          dispatch("setMessages", state.teamMessage.FromUserId);
        });
    },
    setMessageTypesToLookup: () => {
      return new Promise((resolve, reject) =>
        axios
          .get("https://localhost:44369/api/messages/getMessageTypesToLookup")
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
    // setPlayerDetails: async ({ commit }, playerId) => {
    //   await axios
    //     .get("https://localhost:44369/api/players/details", {
    //       params: { id: playerId },
    //     })
    //     .then(function(response) {
    //       commit("SET_PLAYER_DETAILS", response.data);
    //     });
    // },
    // getPlayerDetailsByUserId: async ({ commit }, userId) => {
    //   await axios
    //     .get("https://localhost:44369/api/players/getPlayerByUserId", {
    //       params: { userId: userId },
    //     })
    //     .then(function(response) {
    //       commit("SET_PLAYER_DETAILS", response.data);
    //     });
    // },
  },
};
