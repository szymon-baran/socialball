<template>
  <div
    class="big-data-grid"
    v-if="
      getLoggedInUser.userType === userTypeEnum.TEAM_MANAGEMENT ||
        (userTeamId == null || userTeamId == '')
    "
  >
    <h3>Ogłoszenia poszukiwania drużyny</h3>
    <DxTabPanel>
      <DxItem title="Odpowiedzi na Twoje ogłoszenia">
        <div class="mt-4">
          <JobAdvertisementsAnswersList />
        </div>
      </DxItem>
      <DxItem
        title="Dodaj/edytuj własne ogłoszenie"
        v-if="getLoggedInUser.userType === userTypeEnum.PLAYER"
      >
        <template #default>
          <div class="mt-4">
            <AddEditPlayerAdvertisement />
          </div>
        </template>
      </DxItem>
      <DxItem
        title="Ogłoszenia drużyn, poszukujących zawodników na Twoją pozycję"
        v-if="getLoggedInUser.userType === userTypeEnum.PLAYER"
      >
        <template #default>
          <div class="mt-4">
            <UserAdvertisementsList />
          </div>
        </template>
      </DxItem>
      <DxItem
        title="Zarządzaj ogłoszeniami swojej drużyny"
        v-if="getLoggedInUser.userType === userTypeEnum.TEAM_MANAGEMENT"
      >
        <template #default>
          <div class="mt-4">
            <MyTeamAdvertisementsList />
          </div>
        </template>
      </DxItem>
      <DxItem
        title="Ogłoszenia zawodników bez drużyny"
        v-if="getLoggedInUser.userType === userTypeEnum.TEAM_MANAGEMENT"
      >
        <template #default>
          <div class="mt-4">
            <TeamAdvertisementsList />
          </div>
        </template>
      </DxItem>
    </DxTabPanel>
  </div>
  <div class="big-data-grid text-center" v-else>
    <h3>Strona dostępna wyłącznie dla osób bez drużyny!</h3>
    <DxButton
      text="Przejdź na stronę główną"
      type="default"
      styling-mode="outlined"
      @click="routerPushToHome"
    />
  </div>
</template>
<script>
import DxTabPanel, { DxItem } from "devextreme-vue/tab-panel";
import UserAdvertisementsList from "./forUser/UserAdvertisementsList";
import AddEditPlayerAdvertisement from "./forUser/AddEditPlayerAdvertisement";
import JobAdvertisementsAnswersList from "./JobAdvertisementsAnswersList";
import TeamAdvertisementsList from "./forTeam/TeamAdvertisementsList";
import MyTeamAdvertisementsList from "./forTeam/MyTeamAdvertisementsList";
import { mapGetters, mapActions } from "vuex";
import { userTypeEnum } from "../../enums/userTypeEnum";
import DxButton from "devextreme-vue/button";

export default {
  name: "JobAdvertisements",
  data() {
    return {
      userTypeEnum,
      userTeamId: null,
    };
  },
  computed: {
    ...mapGetters({
      getLoggedInUser: "authentication/getLoggedInUser",
    }),
  },
  methods: {
    ...mapActions({
      getUserTeamId: "authentication/getUserTeamId",
    }),
    routerPushToHome() {
      this.$router.push("/profile");
    },
  },
  mounted() {
    this.getUserTeamId().then((response) => {
      this.userTeamId = response.data;
    });
  },
  components: {
    DxTabPanel,
    DxItem,
    DxButton,
    UserAdvertisementsList,
    AddEditPlayerAdvertisement,
    JobAdvertisementsAnswersList,
    MyTeamAdvertisementsList,
    TeamAdvertisementsList,
  },
};
</script>
