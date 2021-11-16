import axios from "axios";
import { getField, updateField } from "vuex-map-fields";

export default {
  namespaced: true,
  state() {
    return {
      jobAdvertisements: [],
      myJobAdvertisements: [],
      jobAdvertisementsAnswers: [],
      jobAdvertisement: {
        Id: "",
        JobAdvertisementType: null,
        Location: "",
        Content: "",
        TeamId: "",
        LowestEarningsPerMonth: null,
        HighestEarningsPerMonth: null,
        Earnings: "",
        TrainingSessionsPerWeek: null,
        Position: null,
        TeamName: "",
        IsActive: true
      },
      answer: {
        Id: "",
        JobAdvertisementId: "",
        JobAdvertisementAnswerType: null,
        IsResponded: false,
        IsResponsePositive: false,
        ResponseContent: "",
        Content: "",
        TeamId: "",
        UserId: "",
        Team: {},
        User: {},
      },
    };
  },

  getters: {
    getJobAdvertisements(state) {
      return state.jobAdvertisements;
    },
    getJobAdvertisementsAnswers(state) {
      return state.jobAdvertisementsAnswers;
    },
    getMyJobAdvertisements(state) {
      return state.myJobAdvertisements;
    },
    getField,
  },
  mutations: {
    SET_JOB_ADVERTISEMENTS(state, payload) {
      state.jobAdvertisements = payload;
    },
    SET_MY_JOB_ADVERTISEMENTS(state, payload) {
      state.myJobAdvertisements = payload;
    },
    RESET_JOB_ADVERTISEMENTS(state) {
      state.jobAdvertisements = [];
    },
    RESET_MY_JOB_ADVERTISEMENTS(state) {
      state.myJobAdvertisements = [];
    },
    SET_JOB_ADVERTISEMENT_DETAILS(state, payload) {
      state.jobAdvertisement.Id = payload.id;
      state.jobAdvertisement.JobAdvertisementType = payload.jobAdvertisementType;
      state.jobAdvertisement.Location = payload.location;
      state.jobAdvertisement.Content = payload.content;
      state.jobAdvertisement.TeamId = payload.teamId;
      state.jobAdvertisement.LowestEarningsPerMonth = payload.lowestEarningsPerMonth 
      state.jobAdvertisement.HighestEarningsPerMonth = payload.highestEarningsPerMonth;
      state.jobAdvertisement.Earnings = payload.lowestEarningsPerMonth + " - " + payload.highestEarningsPerMonth;
      state.jobAdvertisement.TrainingSessionsPerWeek = payload.trainingSessionsPerWeek;
      state.jobAdvertisement.Position = payload.position;
      state.jobAdvertisement.TeamName = payload.teamName;
      state.jobAdvertisement.IsActive = payload.isActive ?? true;
    }, 
    RESET_JOB_ADVERTISEMENT_FORM(state) {
      state.jobAdvertisement.Id = "";
      state.jobAdvertisement.JobAdvertisementType = null;
      state.jobAdvertisement.Location = "";
      state.jobAdvertisement.Content = "";
      state.jobAdvertisement.TeamId = "";
      state.jobAdvertisement.LowestEarningsPerMonth = null;
      state.jobAdvertisement.HighestEarningsPerMonth = null;
      state.jobAdvertisement.Earnings = "";
      state.jobAdvertisement.TrainingSessionsPerWeek = null;
      state.jobAdvertisement.Position = null;
      state.jobAdvertisement.TeamName = "";
      state.jobAdvertisement.IsActive = true;
    },
    SET_JOB_ADVERTISEMENT_ANSWER_DETAILS(state, payload) {
      state.answer.Id = payload.id;
      state.answer.JobAdvertisementId = payload.jobAdvertisementId;
      state.answer.JobAdvertisementAnswerType = payload.jobAdvertisementAnswerType;
      state.answer.IsResponded = payload.isResponded;
      state.answer.IsResponsePositive = payload.isResponsePositive;
      state.answer.ResponseContent = payload.responseContent;
      state.answer.Content = payload.content;
      state.answer.TeamId = payload.teamId;
      state.answer.UserId = payload.userId;
      state.answer.Team = payload.team;
      state.answer.User = payload.user;
    },
    RESET_JOB_ADVERTISEMENT_ANSWER_FORM(state) {
      state.answer.Id = "";
      state.answer.JobAdvertisementId = "";
      state.answer.JobAdvertisementAnswerType = null;
      state.answer.IsResponded = false;
      state.answer.IsResponsePositive = false;
      state.answer.ResponseContent = "";
      state.answer.Content = "";
      state.answer.TeamId = "";
      state.answer.UserId = "";
      state.answer.Team = "";
      state.answer.User = "";
    },
    RESET_JOB_ADVERTISEMENTS_ANSWERS(state) {
      state.jobAdvertisementsAnswers = [];
    },
    SET_JOB_ADVERTISEMENTS_ANSWERS(state, payload) {
      state.jobAdvertisementsAnswers = payload;
    },
    updateField,
  },

  actions: {
    setUserAdvertisements({ commit }, userId) {
      axios
        .get(
          "https://localhost:44369/api/jobAdvertisements/getUserJobAdvertisements",
          {
            params: { userId: userId },
          }
        )
        .then((response) => {
          commit("SET_JOB_ADVERTISEMENTS", response.data);
        });
    },
    setTeamAdvertisements({ commit }, userId) {
      axios
        .get(
          "https://localhost:44369/api/jobAdvertisements/getTeamJobAdvertisements",
          {
            params: { userId: userId },
          }
        )
        .then((response) => {
          commit("SET_JOB_ADVERTISEMENTS", response.data);
        });
    },
    setMyTeamAdvertisements({ commit }, userId) {
      axios
        .get(
          "https://localhost:44369/api/jobAdvertisements/getMyTeamJobAdvertisements",
          {
            params: { userId: userId },
          }
        )
        .then((response) => {
          commit("SET_MY_JOB_ADVERTISEMENTS", response.data);
        });
    },
    setJobAdvertisementsAnswers({ commit }, userId) {
      axios
        .get(
          "https://localhost:44369/api/jobAdvertisements/getJobAdvertisementsAnswers",
          {
            params: { userId: userId },
          }
        )
        .then((response) => {
          commit("SET_JOB_ADVERTISEMENTS_ANSWERS", response.data);
        });
    },
    getTeamsToLookup() {
      return new Promise((resolve, reject) => {
        axios
          .get("https://localhost:44369/api/jobAdvertisements/getTeamsToLookup")
          .then(
            (response) => {
              resolve(response);
            },
            (error) => {
              reject(error);
            }
          );
      });
    },
    updateUserJobAdvertisement: async ({ state }) => {
      await axios.post(
        "https://localhost:44369/api/jobAdvertisements/updateUserJobAdvertisement",
        state.jobAdvertisement
      );
    },
    setJobAdvertisementDetails: async ({ commit }, id) => {
      await axios
        .get(
          "https://localhost:44369/api/jobAdvertisements/details",
          {
            params: { id: id },
          }
        )
        .then(function(response) {
          commit("SET_JOB_ADVERTISEMENT_DETAILS", response.data);
        });
    },
    setUserJobAdvertisementDetails: async ({ commit }, userId) => {
      await axios
        .get(
          "https://localhost:44369/api/jobAdvertisements/getUserJobAdvertisementDetails",
          {
            params: { userId: userId },
          }
        )
        .then(function(response) {
          commit("SET_JOB_ADVERTISEMENT_DETAILS", response.data);
        });
    },
    setJobAdvertisementAnswerDetails: async ({ commit }, id) => {
      await axios
        .get(
          "https://localhost:44369/api/jobAdvertisements/getJobAdvertisementAnswerDetails",
          {
            params: { id: id },
          }
        )
        .then(function(response) {
          commit("SET_JOB_ADVERTISEMENT_ANSWER_DETAILS", response.data);
        });
    },
    addJobAdvertisementAnswer: async ({ state }) => {
      await axios.post(
        "https://localhost:44369/api/jobAdvertisements/addJobAdvertisementAnswer",
        state.answer
      );
    },
    updateJobAdvertisementAnswer: async ({ state }) => {
      await axios.post(
        "https://localhost:44369/api/jobAdvertisements/updateJobAdvertisementAnswer",
        state.answer
      );
    },
    addMyTeamJobAdvertisement: async ({ state }) => {
      await axios.post(
        "https://localhost:44369/api/jobAdvertisements/addMyTeamJobAdvertisement",
        state.jobAdvertisement
      );
    },
    editMyTeamJobAdvertisement: async ({ state }) => {
      await axios.post(
        "https://localhost:44369/api/jobAdvertisements/editMyTeamJobAdvertisement",
        state.jobAdvertisement
      );
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
  },
};
