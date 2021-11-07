// import axios from "axios";
import { getField, updateField } from "vuex-map-fields";
import AuthService from "../../../services/auth.service";
import router from "../../../router";

const user = JSON.parse(localStorage.getItem("user"));
const initialState = user
  ? {
      status: { loggedIn: true },
      user,
      form: {
        Username: "",
        Password: "",
      },
    }
  : {
      status: { loggedIn: false },
      user: null,
      form: {
        Username: "",
        Password: "",
      },
    };

export default {
  namespaced: true,
  state: initialState,

  getters: {
    getUsername(state) {
      return state.user?.username;
    },
    getLoggedInUser(state) {
      return state.user;
    },
    isLoggedIn(state) {
      return state.status.loggedIn;
    },
    getField,
  },
  mutations: {
    RESET_LOGIN_FORM(state) {
      state.form.Username = "";
      state.form.Password = "";
    },
    // SET_PROFILE(state, profile) {
    //   state.profile = profile;
    // },
    // REFRESH_TOKEN(state) {
    //   state.profile = { ...state.profile, }
    // },

    LOGIN_SUCCESS(state, user) {
      state.status.loggedIn = true;
      state.user = user;
    },
    LOGIN_FAILURE(state) {
      state.status.loggedIn = false;
      state.user = null;
    },
    LOGOUT(state) {
      state.status.loggedIn = false;
      state.user = null;
    },

    updateField,
  },
  actions: {
    login({ state, commit }) {
      return AuthService.login(state.form).then(
        (user) => {
          if (user.ClassName === "System.Exception") {
            commit("LOGIN_FAILURE");
          } else if (user.token) {
            commit("LOGIN_SUCCESS", user);
            commit("RESET_LOGIN_FORM");
          }
          return Promise.resolve(user);
        }
        // (error) => {
        //   commit("LOGIN_FAILURE");
        //   useToast().error("Logowanie nieudane!");
        //   return Promise.resolve(error);
        // }
      );
    },
    logout({ commit }) {
      AuthService.logout();
      commit("LOGOUT");
      router.push({ path: "/" });
    },
  },
};
