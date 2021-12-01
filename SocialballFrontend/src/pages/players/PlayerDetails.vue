<template>
  <div class="big-data-grid">
    <h3>{{ player.FirstName }} {{ player.LastName }}</h3>
    <img
      :src="player.Image"
      alt="Zdjęcie profilowe użytkownika"
      class="avatar"
    />
    <div class="row">
      <div class="col">
        <h4 class="line">Dane osobowe</h4>
        <div class="row">
          <div class="col">
            <ul>
              <li v-if="player.TeamName">
                Aktualna drużyna:
                <router-link
                  :to="{ name: 'teamDetails', params: { id: player.TeamId } }"
                  >{{ player.TeamName }}</router-link
                >
              </li>
              <li>Preferowana pozycja: {{ getPosition() }}</li>
            </ul>
          </div>
          <div class="col">
            <li v-if="player.Citizenship">
              Narodowość: {{ player.Citizenship }}
            </li>
            <li v-if="player.Email">E-mail kontaktowy: {{ player.Email }}</li>
          </div>
        </div>
      </div>
    </div>
    <div class="mt-2" v-if="isLoggedIn">
      <DxButton
        text="Pokaż statystyki"
        @click="showStats()"
        ref="showStatsButton"
        type="default"
      />
    </div>
    <div class="row" v-if="areStatsVisible">
      <div class="col">
        <h4 class="line">Strzelone bramki ({{ player.Goals.length }})</h4>
        <DxDataGrid
          :data-source="player.Goals"
          :remote-operations="false"
          :row-alternation-enabled="true"
          :show-borders="true"
          :column-auto-width="true"
          width="100%"
          no-data-text="Brak strzelonych goli"
        >
          <DxLoadPanel :enabled="true" />
          <DxColumn data-field="matchBetween" caption="Mecz pomiędzy" />
          <DxColumn data-field="minute" caption="Minuta" />
          <DxColumn data-field="goalAssistPlayerName" caption="Asystent" />
          <DxColumn
            data-field="dateTime"
            caption="Data"
            data-type="date"
            format="dd/MM/yyyy"
          />
          <DxPaging :page-size="10" />
          <DxPager :visible="true" :allowed-page-sizes="[5, 10]" />
        </DxDataGrid>
      </div>
      <div class="col">
        <h4 class="line text-right">Asystowane bramki ({{ player.Assists.length }})</h4>
        <DxDataGrid
          :data-source="player.Assists"
          :remote-operations="false"
          :row-alternation-enabled="true"
          :show-borders="true"
          :column-auto-width="true"
          width="100%"
          no-data-text="Brak asyst"
        >
          <DxLoadPanel :enabled="true" />
          <DxColumn data-field="matchBetween" caption="Mecz pomiędzy" />
          <DxColumn data-field="minute" caption="Minuta" />
          <DxColumn data-field="goalScorerName" caption="Strzelec" />
          <DxColumn
            data-field="dateTime"
            caption="Data"
            data-type="date"
            format="dd/MM/yyyy"
          />
          <DxPaging :page-size="10" />
          <DxPager :visible="true" :allowed-page-sizes="[5, 10]" />
        </DxDataGrid>
      </div>
    </div>
    <div class="mt-4" v-if="isLoggedIn">
      <DxButton text="Kontakt" @click="showAddMessagePopup()" />
    </div>
    <MessageAdd
      v-if="addMessagePopupOptions.isVisible"
      :messageType="0"
      :userIdFromProfile="player.UserId"
      @close="onAddMessagePopupClose"
    />
  </div>
</template>

<script>
import { mapState, mapActions, mapGetters, mapMutations } from "vuex";
import DxButton from "devextreme-vue/button";
import {
  DxDataGrid,
  DxLoadPanel,
  DxColumn,
  DxPager,
  DxPaging,
} from "devextreme-vue/data-grid";
import MessageAdd from "../messages/MessageAdd";

export default {
  name: "PlayerDetails",
  data() {
    return {
      areStatsVisible: false,
      addMessagePopupOptions: {
        isVisible: false,
      },
    };
  },
  computed: {
    ...mapGetters({
      getTeams: "teams/getTeams",
      isLoggedIn: "authentication/isLoggedIn",
    }),
    ...mapState("players", ["player"]),
  },
  methods: {
    ...mapActions({
      setPlayerDetails: "players/setPlayerDetails",
      setAllTeams: "teams/setAllTeams",
    }),
    ...mapMutations({
      RESET_PLAYER_FORM: "players/RESET_PLAYER_FORM",
    }),
    getPosition() {
      switch (this.player.Position) {
        case 0:
          return "Bramkarz";
        case 1:
          return "Obrońca";
        case 2:
          return "Pomocnik";
        case 3:
          return "Napastnik";
      }
    },
    showAddMessagePopup() {
      this.addMessagePopupOptions.isVisible = true;
    },
    onAddMessagePopupClose() {
      this.addMessagePopupOptions.isVisible = false;
    },
    showStats() {
      const buttonInstance = this.$refs["showStatsButton"].instance;
      if (!this.areStatsVisible) {
        this.areStatsVisible = true;
        buttonInstance.option("text", "Ukryj statystyki");
        buttonInstance.option("stylingMode", "outlined");
      } else {
        this.areStatsVisible = false;
        buttonInstance.option("text", "Pokaż statystyki");
        buttonInstance.option("stylingMode", "contained");
      }
    },
  },
  mounted() {
    this.setPlayerDetails(this.$route.params.id);
    this.setAllTeams();
  },
  components: {
    DxButton,
    DxDataGrid,
    DxLoadPanel,
    DxColumn,
    DxPager,
    DxPaging,
    MessageAdd,
  },
  beforeUnmount() {
    this.RESET_PLAYER_FORM();
  },
};
</script>
