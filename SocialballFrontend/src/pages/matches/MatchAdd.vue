<template>
  <div>
    <DxPopup
      v-model:visible="popupVisible"
      :drag-enabled="false"
      :close-on-outside-click="false"
      :show-close-button="false"
      :show-title="true"
      width="95%"
      height="80%"
      container=".dx-viewport"
      title="Dodaj mecz"
      :shading="false"
    >
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="after"
        :options="sendButtonOptions"
      />
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="before"
        :options="closeButtonOptions"
      />
      <DxScrollView width="100%" height="100%">
        <form>
          <DxValidationGroup :ref="groupRefKey">
            <div class="row">
              <div class="col">
                <label for="homeTeamIdSelectBox" class="form-label"
                  >Drużyna domowa</label
                >
                <DxSelectBox
                  :dataSource="getTeams"
                  value-expr="id"
                  display-expr="name"
                  v-model="HomeTeamId"
                  id="homeTeamIdSelectBox"
                  @value-changed="onHomeTeamChange"
                >
                  <DxValidator>
                    <DxRequiredRule message="Drużyna domowa jest wymagana!" />
                  </DxValidator>
                </DxSelectBox>
              </div>
              <div class="col">
                <label for="awayTeamIdSelectBox" class="form-label"
                  >Drużyna wyjazdowa</label
                >
                <DxSelectBox
                  :dataSource="getTeams"
                  value-expr="id"
                  display-expr="name"
                  v-model="AwayTeamId"
                  id="awayTeamIdSelectBox"
                  @value-changed="onAwayTeamChange"
                >
                  <DxValidator>
                    <DxRequiredRule
                      message="Drużyna wyjazdowa jest wymagana!"
                    />
                  </DxValidator>
                </DxSelectBox>
              </div>
              <DxValidator :adapter="teamValidatorConfig">
                <DxRequiredRule
                  message="Możesz dodać wyłącznie mecz, w którym uczestniczy Twoja drużyna i nie może grać sama ze sobą!"
                />
              </DxValidator>
              <DxValidator :adapter="leagueValidatorConfig">
                <DxRequiredRule
                  message="W przypadku rozgrywania meczu ligowego, obie drużyny muszą należeć do tej samej ligi."
                />
              </DxValidator>
            </div>
            <div class="row mt-4">
              <div class="col">
                <label for="matchTypeSelectBox" class="form-label"
                  >Typ meczu</label
                >
                <DxSelectBox
                  :dataSource="matchTypes"
                  value-expr="value"
                  display-expr="name"
                  v-model="MatchType"
                  id="matchTypeSelectBox"
                >
                  <DxValidator>
                    <DxRequiredRule message="Typ meczu jest wymagany!" />
                  </DxValidator>
                </DxSelectBox>
              </div>
              <div class="col">
                <label for="dateTimeTextBox" class="form-label"
                  >Data i godzina meczu</label
                >
                <DxDateBox
                  id="dateTimeTextBox"
                  v-model="DateTime"
                  type="datetime"
                  display-format="dd/MM/yyyy HH:mm"
                  cancel-button-text="Anuluj"
                  invalid-date-message="Wartość musi być datą lub czasem"
                />
              </div>
              <div class="col">
                <label for="stadiumTextBox" class="form-label">Stadion</label>
                <DxTextBox id="stadiumTextBox" v-model="Stadium">
                  <DxValidator>
                    <DxRequiredRule message="Podaj nazwę stadionu!" />
                  </DxValidator>
                </DxTextBox>
              </div>
            </div>
            <div class="mt-4" v-if="DateTime != null && DateTime < Date.now()">
              <div class="row">
                <div class="col">
                  <h4>Składy drużyn</h4>
                </div>
              </div>
              <div class="row mt-2">
                <div class="col">
                  <p>Drużyna domowa ({{ HomeMatchPlayers.length }}/11)</p>
                  <DxDataGrid
                    :data-source="HomeMatchPlayers"
                    :remote-operations="false"
                    :row-alternation-enabled="true"
                    :show-borders="true"
                    :hover-state-enabled="true"
                    :column-auto-width="true"
                    width="100%"
                    ref="homeTeamDataGrid"
                    no-data-text="Brak wybranych zawodników"
                  >
                    <DxEditing
                      :allow-updating="false"
                      :allow-deleting="true"
                      :allow-adding="true"
                      mode="row"
                    />
                    <DxColumn data-field="PlayerId" caption="Zawodnik">
                      <DxLookup
                        :data-source="homeTeamPlayers"
                        display-expr="name"
                        value-expr="id"
                      />
                      <DxDataGridCustomRule
                        :validation-callback="validateInsertedPlayerHome"
                        message="Ten zawodnik już został dodany do składu"
                      />
                      <DxDataGridRequiredRule message="Wybierz zawodnika!" />
                    </DxColumn>
                    <DxColumn
                      data-field="Position"
                      caption="Pozycja w meczu"
                      :sort-index="0"
                      sort-order="asc"
                    >
                      <DxLookup
                        :data-source="positions"
                        display-expr="name"
                        value-expr="value"
                      />
                      <DxDataGridRequiredRule message="Wybierz pozycję!" />
                    </DxColumn>
                    <DxColumn data-field="Number" caption="Numer zawodnika" />
                  </DxDataGrid>
                </div>
                <div class="col">
                  <p>Drużyna wyjazdowa ({{ AwayMatchPlayers.length }}/11)</p>
                  <DxDataGrid
                    :data-source="AwayMatchPlayers"
                    :remote-operations="false"
                    :row-alternation-enabled="true"
                    :show-borders="true"
                    :hover-state-enabled="true"
                    :column-auto-width="true"
                    width="100%"
                    ref="awayTeamDataGrid"
                    no-data-text="Brak wybranych zawodników"
                  >
                    <DxSorting mode="none" />
                    <DxEditing
                      :allow-updating="false"
                      :allow-deleting="true"
                      :allow-adding="true"
                      mode="row"
                    />
                    <DxColumn data-field="PlayerId" caption="Zawodnik">
                      <DxLookup
                        :data-source="awayTeamPlayers"
                        display-expr="name"
                        value-expr="id"
                      />
                      <DxDataGridRequiredRule message="Wybierz zawodnika!" />
                      <DxDataGridCustomRule
                        :validation-callback="validateInsertedPlayerAway"
                        message="Ten zawodnik już został dodany do składu"
                      />
                    </DxColumn>
                    <DxColumn
                      data-field="Position"
                      caption="Pozycja w meczu"
                      :sort-index="0"
                      sort-order="asc"
                    >
                      <DxLookup
                        :data-source="positions"
                        display-expr="name"
                        value-expr="value"
                      />
                      <DxDataGridRequiredRule message="Wybierz pozycję!" />
                    </DxColumn>
                    <DxColumn data-field="Number" caption="Numer zawodnika" />
                  </DxDataGrid>
                </div>
              </div>
              <div class="row mt-4">
                <div class="col">
                  <h4>Zdarzenia meczowe</h4>
                  <DxDataGrid
                    :data-source="MatchEvents"
                    :remote-operations="false"
                    :row-alternation-enabled="true"
                    :show-borders="true"
                    :hover-state-enabled="true"
                    :column-auto-width="true"
                    width="100%"
                    @editor-preparing="onEditorPreparing"
                    ref="matchEventsDataGrid"
                    no-data-text="Brak zdarzeń"
                  >
                    <DxSorting mode="none" />
                    <DxEditing
                      :allow-updating="true"
                      :allow-deleting="true"
                      :allow-adding="true"
                      mode="row"
                    />
                    <DxColumn
                      data-field="MatchEventType"
                      caption="Typ zdarzenia"
                    >
                      <DxLookup
                        :data-source="eventTypes"
                        display-expr="name"
                        value-expr="value"
                      />
                      <DxDataGridRequiredRule
                        message="Wybierz typ zdarzenia!"
                      />
                    </DxColumn>
                    <DxColumn data-field="EventTeamId" caption="Drużyna">
                      <DxLookup
                        :data-source="teamsSelected"
                        display-expr="name"
                        value-expr="id"
                      />
                      <DxDataGridRequiredRule message="Wybierz drużynę!" />
                    </DxColumn>
                    <DxColumn data-field="PlayerId" caption="Zawodnik">
                      <DxLookup
                        :data-source="players"
                        display-expr="name"
                        value-expr="id"
                        :show-clear-button="true"
                      />
                      <DxDataGridRequiredRule message="Wybierz zawodnika!" />
                    </DxColumn>
                    <DxColumn data-field="Minute" caption="Minuta" />
                    <DxColumn data-field="AssistPlayerId" caption="Asystujący">
                      <DxLookup
                        :data-source="players"
                        display-expr="name"
                        value-expr="id"
                      />
                    </DxColumn>
                    <DxColumn data-field="PenaltyType" caption="Rodzaj kary">
                      <DxLookup
                        :data-source="penaltyTypes"
                        display-expr="name"
                        value-expr="value"
                      />
                    </DxColumn>
                  </DxDataGrid>
                </div>
              </div>
            </div>
            <div class="row mt-4">
              <div class="col">
                <DxValidationSummary />
              </div>
            </div>
          </DxValidationGroup>
        </form>
        <h4 class="mt-4 text-center">
          Aby mecz został dodany, członek zarządu przeciwnej drużyny musi go
          zatwierdzić.
        </h4>
      </DxScrollView>
    </DxPopup>
  </div>
</template>
<script>
import { DxPopup, DxToolbarItem } from "devextreme-vue/popup";
import DxTextBox from "devextreme-vue/text-box";
import DxSelectBox from "devextreme-vue/select-box";
import { mapActions, mapGetters, mapMutations } from "vuex";
import { createHelpers } from "vuex-map-fields";
import { DxValidator, DxRequiredRule } from "devextreme-vue/validator";
const { mapFields } = createHelpers({
  getterType: "matches/getField",
  mutationType: "matches/updateField",
});
import DxValidationGroup from "devextreme-vue/validation-group";
import DxValidationSummary from "devextreme-vue/validation-summary";
import { useToast } from "vue-toastification";
import DxDateBox from "devextreme-vue/date-box";
import {
  DxDataGrid,
  DxColumn,
  DxEditing,
  DxLookup,
  DxSorting,
  DxRequiredRule as DxDataGridRequiredRule,
  DxCustomRule as DxDataGridCustomRule,
} from "devextreme-vue/data-grid";
import { DxScrollView } from "devextreme-vue/scroll-view";

export default {
  name: "MatchAdd",
  data() {
    const callbacks = [];
    const teamValidatorConfig = {
      getValue: () => {
        return (
          (this.HomeTeamId === this.userTeamId ||
            this.AwayTeamId === this.userTeamId) &&
          this.HomeTeamId !== this.AwayTeamId
        );
      },
      validationRequestsCallbacks: callbacks,
    };
    const leagueValidatorConfig = {
      getValue: () => {
        if (this.MatchType === 1) {
          return this.validateLeagueInTeams();
        } else {
          return true;
        }
      },
      validationRequestsCallbacks: callbacks,
    };
    return {
      eventTypes: [],
      players: [],
      penaltyTypes: [],
      teamsSelected: [],
      matchTypes: [],
      positions: [],
      popupVisible: false,
      sendButtonOptions: {
        text: "Dodaj",
        onClick: () => this.handleSubmit(),
      },
      closeButtonOptions: {
        text: "Zamknij",
        onClick: () => {
          this.popupVisible = false;
          this.$emit("close");
        },
        type: "normal",
      },
      groupRefKey: "targetGroup",
      userTeamId: null,
      teamValidatorConfig,
      leagueValidatorConfig,
      homeTeamPlayers: [],
      awayTeamPlayers: [],
    };
  },
  computed: {
    ...mapGetters({
      getLoggedInUser: "authentication/getLoggedInUser",
      getTeams: "teams/getTeams",
    }),
    ...mapFields([
      "match.HomeTeamId",
      "match.AwayTeamId",
      "match.Stadium",
      "match.DateTime",
      "match.MatchType",
      "match.MatchEvents",
      "match.AddedByTeamId",
      "match.HomeMatchPlayers",
      "match.AwayMatchPlayers",
    ]),
    validationGroup: function() {
      return this.$refs[this.groupRefKey].instance;
    },
  },
  methods: {
    ...mapActions({
      addMatch: "matches/addMatch",
      getUserTeamId: "authentication/getUserTeamId",
      setAllTeams: "teams/setAllTeams",
      getEventTypesToLookup: "matches/getEventTypesToLookup",
      getPenaltyTypesToLookup: "matches/getPenaltyTypesToLookup",
      getMatchTypesLookup: "matches/getMatchTypesLookup",
      getPlayersByTeam: "matches/getPlayersByTeam",
      setPositionsToLookup: "teams/setPositionsToLookup",
    }),
    ...mapMutations({
      RESET_MATCH_FORM: "matches/RESET_MATCH_FORM",
    }),
    validateTeamsSquads() {
      if (
        this.HomeMatchPlayers.length != 11 ||
        this.AwayMatchPlayers.length != 11
      ) {
        useToast().error("W drużynie musi grać dokładnie 11 zawodników!");
        return false;
      }
      let homeGoalkeepers = this.HomeMatchPlayers.filter(
        (x) => x.Position == 0
      );
      let awayGoalkeepers = this.AwayMatchPlayers.filter(
        (x) => x.Position == 0
      );
      if (homeGoalkeepers.length != 1 || awayGoalkeepers.length != 1) {
        useToast().error("W każdej drużynie musi grać jeden bramkarz!");
        return false;
      }

      return true;
    },
    async handleSubmit() {
      let validationResult = this.validationGroup.validate();
      if (validationResult.isValid && this.validateTeamsSquads()) {
        await this.addMatch(this.userTeamId);
        useToast().success(
          "Mecz został dodany pomyślnie! Teraz musisz poczekać na akceptację ze strony drużyny przeciwnej."
        );
        this.popupVisible = false;
        this.$emit("close");
      }
    },
    onEditorPreparing(e) {
      if (e.dataField === "EventTeamId") {
        var onValueChanged = e.editorOptions.onValueChanged;
        const context = this;
        e.editorOptions.onValueChanged = function(args) {
          onValueChanged.apply(this, arguments);
          if (args.value == context.HomeTeamId) {
            context.players = context.homeTeamPlayers;
          } else if (args.value == context.AwayTeamId) {
            context.players = context.awayTeamPlayers;
          }
        };
      }
    },
    onHomeTeamChange() {
      this.HomeMatchPlayers = [];
      this.teamsSelected = this.getTeams.filter(
        (x) => x.id == this.HomeTeamId || x.id == this.AwayTeamId
      );
      this.getPlayersByTeam(this.HomeTeamId).then((response) => {
        this.homeTeamPlayers = response.data;
      });
    },
    onAwayTeamChange() {
      this.AwayMatchPlayers = [];
      this.teamsSelected = this.getTeams.filter(
        (x) => x.id == this.HomeTeamId || x.id == this.AwayTeamId
      );
      this.getPlayersByTeam(this.AwayTeamId).then((response) => {
        this.awayTeamPlayers = response.data;
      });
    },
    validateLeagueInTeams() {
      let homeTeamLeagueId = this.HomeTeamId
        ? this.getTeams.find((x) => x.id === this.HomeTeamId).leagueId
        : null;
      let awayTeamLeagueId = this.AwayTeamId
        ? this.getTeams.find((x) => x.id === this.AwayTeamId).leagueId
        : null;
      if (homeTeamLeagueId === awayTeamLeagueId) {
        return true;
      } else {
        return false;
      }
    },
    validateInsertedPlayerHome(e) {
      if (this.HomeMatchPlayers.find((x) => x.PlayerId == e.value)) {
        return false;
      } else {
        return true;
      }
    },
    validateInsertedPlayerAway(e) {
      if (this.AwayMatchPlayers.find((x) => x.PlayerId == e.value)) {
        return false;
      } else {
        return true;
      }
    },
  },
  components: {
    DxPopup,
    DxToolbarItem,
    DxTextBox,
    DxSelectBox,
    DxValidationGroup,
    DxValidationSummary,
    DxDateBox,
    DxValidator,
    DxRequiredRule,
    DxDataGrid,
    DxColumn,
    DxEditing,
    DxLookup,
    DxSorting,
    DxDataGridRequiredRule,
    DxDataGridCustomRule,
    DxScrollView,
  },
  mounted() {
    this.getUserTeamId().then((response) => {
      this.userTeamId = response.data;
      this.AddedByTeamId = response.data;
    });
    this.getEventTypesToLookup().then((response) => {
      this.eventTypes = response.data;
    });
    this.getPenaltyTypesToLookup().then((response) => {
      this.penaltyTypes = response.data;
    });
    this.getMatchTypesLookup().then((response) => {
      this.matchTypes = response.data;
    });
    this.setPositionsToLookup().then((response) => {
      this.positions = response.data;
    });

    this.setAllTeams();
    this.popupVisible = true;
  },
  beforeUnmount() {
    this.RESET_MATCH_FORM();
  },
};
</script>
