<template>
  <div class="big-data-grid">
    <div v-if="getPlayerDetails.Id" class="text-center mb-3">
      <h3>
        Witaj, {{ getPlayerDetails.FirstName }} {{ getPlayerDetails.LastName }}!
      </h3>
      <router-link
        :to="{ name: 'playerDetails', params: { id: getPlayerDetails.Id } }"
        v-if="getLoggedInUser.userType === 1"
      >
        <img
          :src="getPlayerDetails.Image"
          alt="Zdjęcie profilowe użytkownika"
          class="avatar"
        />
      </router-link>
      <img
        :src="getPlayerDetails.Image"
        alt="Zdjęcie profilowe użytkownika"
        class="avatar"
        v-else
      />
    </div>
    <div v-if="userTeamId" class="text-center">
      <h4>
        Twoja drużyna:
        <router-link
          :to="{ name: 'teamDetails', params: { id: userTeamId } }"
          >{{ getPlayerDetails.TeamName }}</router-link
        >
      </h4>
      <p class="main-color" v-if="getLoggedInUser.userType === 2">Konto zarządu drużyny</p>
      <p v-if="getPlayerDetails.Earnings">Twoje aktualne zarobki miesięczne: {{ getPlayerDetails.Earnings }} PLN.</p>
    </div>
    <div v-else class="text-center">
      <p>
        Aktualnie nie posiadasz drużyny. <br />Nasza strona udostępnia
        rozwiązanie umożliwiające zmianę tej sytuacji! Kliknij przycisk poniżej.
      </p>
      <DxButton
        text="Poszukiwanie drużyny"
        type="default"
        styling-mode="outlined"
        @click="routerPushToJobAdvertisements"
      />
    </div>
    <div class="primary-border">
      <h4 class="line">Co chcesz zrobić?</h4>
      <div class="row mb-4 text-center">
        <div class="col-6 col-md-3 mb-3">
          <DxButton
            text="Edytuj profil"
            type="default"
            @click="routerPushToEditProfile"
            width="300px"
            height="150px"
          />
        </div>
        <div class="col-6 col-md-3 mb-3">
          <DxButton
            text="Baza zawodników"
            type="default"
            @click="routerPushToPlayers"
            width="300px"
            height="150px"
          />
        </div>
        <div class="col-6 col-md-3 mb-3">
          <DxButton
            text="Tabele ligowe"
            type="default"
            @click="routerPushToTeams"
            width="300px"
            height="150px"
          />
        </div>
        <div class="col-6 col-md-3 mb-3">
          <DxButton
            text="Baza meczy"
            type="default"
            @click="routerPushToMatches"
            width="300px"
            height="150px"
          />
        </div>
      </div>
    </div>
    <div v-if="getLoggedInUser.userType === 2" class="secondary-border">
      <h4 class="line">Funkcje zarządu drużyny</h4>
      <div class="row text-center">
        <div class="col-6 col-md-3 mb-3">
          <DxButton
            text="Zarządzanie drużyną"
            type="danger"
            styling-mode="outlined"
            @click="routerPushToInjuriesList"
            width="300px"
            height="150px"
          />
        </div>
        <div class="col-6 col-md-3 mb-3">
          <DxButton
            text="Ogłoszenia zawodników"
            type="danger"
            styling-mode="outlined"
            @click="routerPushToJobAdvertisements"
            width="300px"
            height="150px"
          />
        </div>
        <div class="col-6 col-md-3 mb-3">
          <DxButton
            text="Niepotwierdzone mecze"
            type="danger"
            styling-mode="outlined"
            @click="routerPushToUnconfirmedMatches"
            width="300px"
            height="150px"
          />
        </div>
        <div class="col-6 col-md-3 mb-3">
          <DxButton
            text="Transfery drużyny"
            type="danger"
            styling-mode="outlined"
            @click="routerPushToTransferOffers"
            width="300px"
            height="150px"
          />
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import { mapGetters, mapActions, mapMutations } from "vuex";
import { userTypeEnum } from "../../enums/userTypeEnum";
import DxButton from "devextreme-vue/button";

export default {
  name: "UserProfile",
  data() {
    return {
      userTypeEnum,
      userTeamId: null,
    };
  },
  computed: {
    ...mapGetters({
      getLoggedInUser: "authentication/getLoggedInUser",
      getPlayerDetails: "players/getPlayerDetails",
    }),
  },
  methods: {
    ...mapActions({
      getPlayerDetailsByUserId: "players/getPlayerDetailsByUserId",
      getUserDataByUserId: "players/getUserDataByUserId",
      getUserTeamId: "authentication/getUserTeamId",
    }),
    ...mapMutations({
      RESET_PLAYER_FORM: "players/RESET_PLAYER_FORM",
    }),
    routerPushToEditProfile() {
      this.$router.push({
        name: "editProfile",
        params: { playerToEditId: this.getPlayerDetails.Id },
      });
    },
    routerPushToPlayers() {
      this.$router.push({ path: `/players` });
    },
    routerPushToTeams() {
      this.$router.push({ path: `/teams` });
    },
    routerPushToMatches() {
      this.$router.push({ path: `/matches` });
    },
    routerPushToInjuriesList() {
      this.$router.push({ path: `/team-management` });
    },
    routerPushToJobAdvertisements() {
      this.$router.push({ path: `/job-advertisements` });
    },
    routerPushToUnconfirmedMatches() {
      this.$router.push({ path: `/unconfirmed-matches` });
    },
    routerPushToTransferOffers() {
      this.$router.push({ path: `/transfer-offers` });
    },
  },
  mounted() {
    this.getUserTeamId().then((response) => {
      this.userTeamId = response.data;
    });
    if (this.getLoggedInUser) {
      switch (this.getLoggedInUser.userType) {
        case userTypeEnum.PLAYER:
          this.getPlayerDetailsByUserId(this.getLoggedInUser.id);
          break;
        default:
          this.getUserDataByUserId(this.getLoggedInUser.id);
          break;
      }
    }
  },
  components: {
    DxButton,
  },
  beforeUnmount() {
    this.RESET_PLAYER_FORM();
  },
};
</script>
