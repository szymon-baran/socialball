import axios from "axios";
import { getField, updateField } from "vuex-map-fields";
import authHeader from "../../../services/auth-header";

export default {
  namespaced: true,
  state() {
    return {
      unreadMessagesCount: 0,
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
    SET_MESSAGES(state, { payload, areSentMessages }) {
      state.messages = payload;
      if (!areSentMessages) {
        state.messages.some((object) => {
          if (object.isRead === false) {
            state.unreadMessagesCount++;
          }
        });
      }
    },
    RESET_MESSAGES(state) {
      state.messages = [];
    },
    RESET_UNREAD_MESSAGES_COUNT(state) {
      state.unreadMessagesCount = 0;
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
          commit("SET_MESSAGES", {
            payload: response.data,
            areSentMessages: false,
          });
        });
    },
    setSentMessages({ commit }, userId) {
      axios
        .get("https://localhost:44369/api/messages/getSentMessages", {
          params: { userId: userId },
          headers: authHeader(),
        })
        .then((response) => {
          commit("SET_MESSAGES", {
            payload: response.data,
            areSentMessages: true,
          });
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
    markMessageAsRead: async ({ dispatch, commit }, message) => {
      await axios
        .post("https://localhost:44369/api/messages/markMessageAsRead", {
          Id: message.id,
        })
        .then(() => {
          commit("RESET_UNREAD_MESSAGES_COUNT");
          dispatch("setMessages", message.toUserId);
        });
    },
    deleteMessage: async ({ dispatch, commit }, message) => {
      await axios
        .post("https://localhost:44369/api/messages/deleteMessage", {
          Id: message.id,
        })
        .then(() => {
          commit("RESET_UNREAD_MESSAGES_COUNT");
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
  },
};
