import axios from "axios";
import { getField, updateField } from "vuex-map-fields";
import authHeader from "../../../services/auth-header";

export default {
  namespaced: true,
  state() {
    return {
      messages: [],
      message: {
        FromUserId: "",
        ToUserId: "",
        ToTeamId: "",
        Subject: "",
        Content: "",
        SentOn: null,
        MessageType: null,
      },
    };
  },
  getters: {
    getMessages(state) {
      return state.messages;
    },
    getMessageDetails(state) {
      return state.message;
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
      state.message.FromUserId = payload.fromUserId;
      state.message.ToUserId = payload.toUserId;
      state.message.ToTeamId = payload.toTeamId;
      state.message.Subject = payload.subject;
      state.message.Content = payload.content;
      state.message.SentOn = payload.sentOn;
    },
    RESET_TEAM_MESSAGE_FORM(state) {
      state.message.FromUserId = "";
      state.message.ToUserId = "";
      state.message.ToTeamId = "";
      state.message.Subject = "";
      state.message.Content = "";
      state.message.SentOn = null;
      state.message.MessageType = null;
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
    sendMessage: async ({ state, dispatch }) => {
      const userId = state.message.FromUserId;
      await axios
        .post(
          "https://localhost:44369/api/messages/addMessage",
          state.message,
          {
            headers: authHeader(),
          }
        )
        .then(() => {
          dispatch("setMessages", userId);
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
    markMessageAsRead: async ({ dispatch }, message) => {
      await axios
        .post("https://localhost:44369/api/messages/markMessageAsRead", {
          Id: message.id,
        })
        .then(() => {
          dispatch("setMessages", message.toUserId);
        });
    },
    deleteMessage: async ({ dispatch }, message) => {
      await axios
        .post("https://localhost:44369/api/messages/deleteMessage", {
          Id: message.id,
        })
        .then(() => {
          dispatch("setMessages", message.toUserId);
        });
    },
    getUsersToLookup: () => {
      return new Promise((resolve, reject) => {
        axios.get("https://localhost:44369/api/messages/getUsersToLookup").then(
          (response) => {
            resolve(response);
          },
          (error) => {
            reject(error);
          }
        );
      });
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
