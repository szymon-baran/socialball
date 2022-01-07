<template>
  <div>
    <DxPopup
      v-model:visible="popupVisible"
      :drag-enabled="false"
      :close-on-outside-click="false"
      :show-close-button="false"
      :show-title="true"
      :width="600"
      :height="410"
      container=".dx-viewport"
      title="Edytuj drużynę"
      :shading="false"
    >
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="after"
        :options="sendButtonOptionsAccept"
      />
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="before"
        :options="closeButtonOptions"
      />
      <form>
        <DxValidationGroup :ref="groupRefKey">
          <div class="row">
            <div class="col">
              <label for="nameTextBox" class="form-label">Nazwa</label>
              <DxTextBox v-model="Name" id="nameTextBox" :read-only="true">
                <DxValidator>
                  <DxRequiredRule message="Podaj nazwę!" />
                </DxValidator>
              </DxTextBox>
            </div>
            <div class="col">
              <label for="leagueIdSelectBox" class="form-label">Liga</label>
              <DxSelectBox
                :dataSource="leagues"
                value-expr="id"
                display-expr="name"
                v-model="LeagueId"
                id="leagueIdSelectBox"
              >
                <DxValidator>
                  <DxRequiredRule message="Wybór ligi jest wymagany!" />
                </DxValidator>
              </DxSelectBox>
            </div>
          </div>
          <div class="row mt-4">
            <div class="col">
              <DxCheckBox v-model:value="IsActive" id="isActiveCheckBox" />
              <label for="isActiveCheckBox" class="form-label ml-2"
                >Czy drużyna jest aktywna?</label
              >
            </div>
          </div>
        </DxValidationGroup>
        <div class="mb-4">
          <h4 class="line">Opcje</h4>
          <div class="row">
            <div class="col">
              <DxButton
                text="Usuń drużynę"
                @click="handleDelete()"
                type="danger"
                styling-mode="outlined"
                class="mb-4"
              />
            </div>
          </div>
          <div class="row">
            <div class="col">
              <DxButton
                text="Usuń zdjęcie drużyny"
                @click="handlePhotoDelete()"
                type="danger"
                styling-mode="text"
                class="mb-4"
              />
            </div>
          </div>
        </div>
      </form>
    </DxPopup>
  </div>
</template>
<script>
import { DxValidator, DxRequiredRule } from "devextreme-vue/validator";
import DxValidationGroup from "devextreme-vue/validation-group";
import { DxPopup, DxToolbarItem } from "devextreme-vue/popup";
import DxSelectBox from "devextreme-vue/select-box";
import DxButton from "devextreme-vue/button";
import DxTextBox from "devextreme-vue/text-box";
import DxCheckBox from "devextreme-vue/check-box";

import { mapActions, mapMutations } from "vuex";
import { createHelpers } from "vuex-map-fields";
const { mapFields } = createHelpers({
  getterType: "teams/getField",
  mutationType: "teams/updateField",
});

import { useToast } from "vue-toastification";
import { custom } from "devextreme/ui/dialog";

export default {
  name: "AdminTeamEdit",
  props: {
    teamId: {
      type: String,
      required: false,
    },
    selectedLeagueId: {
      type: String,
      required: false,
    },
  },
  data() {
    return {
      popupVisible: false,
      leagues: [],
      sendButtonOptionsAccept: {
        icon: "check",
        text: "Zapisz",
        onClick: () => {
          this.handleSubmit();
        },
      },
      closeButtonOptions: {
        text: "Zamknij",
        onClick: () => {
          this.popupVisible = false;
          this.$emit("closed");
        },
        type: "normal",
      },
      groupRefKey: "targetGroup",
    };
  },
  computed: {
    ...mapFields(["team.Id", "team.Name", "team.LeagueId", "team.IsActive"]),
    validationGroup: function() {
      return this.$refs[this.groupRefKey].instance;
    },
  },
  methods: {
    ...mapActions({
      setTeamDetails: "teams/setTeamDetails",
      setLeaguesToLookup: "teams/setLeaguesToLookup",
      editTeamAdmin: "teams/editTeamAdmin",
      teamDeleteAdmin: "teams/teamDeleteAdmin",
      teamImageDeleteAdmin: "teams/teamImageDeleteAdmin",
    }),
    ...mapMutations({
      RESET_TEAM_DETAILS: "teams/RESET_TEAM_DETAILS",
    }),
    handleDelete() {
      let dialog = custom({
        title: "Potwierdzenie",
        messageHtml: `Czy na pewno chcesz usunąć drużynę ${this.Name}?<br>Ta operacja jest nieodwracalna, a wszyscy członkowie drużyny zostaną z niej usunięci.<br>Jeśli nie jesteś tego pewien, lepszym rozwiązaniem jest dezaktywacja drużyny.`,
        buttons: [
          {
            text: "Tak",
            onClick: () => {
              return true;
            },
          },
          {
            text: "Nie",
            onClick: () => {
              return false;
            },
          },
        ],
      });
      dialog.show().then((dialogResult) => {
        if (dialogResult === true) {
          this.teamDeleteAdmin(this.selectedLeagueId);
          this.popupVisible = false;
          this.$emit("closed");
          useToast().success("Drużyna została usunięta!");
        }
      });
    },
    handlePhotoDelete() {
      let dialog = custom({
        title: "Potwierdzenie",
        messageHtml: `Czy na pewno chcesz usunąć zdjęcie drużyny ${this.Name}?`,
        buttons: [
          {
            text: "Tak",
            onClick: () => {
              return true;
            },
          },
          {
            text: "Nie",
            onClick: () => {
              return false;
            },
          },
        ],
      });
      dialog.show().then((dialogResult) => {
        if (dialogResult === true) {
          this.teamImageDeleteAdmin();
          this.popupVisible = false;
          this.$emit("closed");
          useToast().success("Zdjęcie drużyny zostało usunięte!");
        }
      });
    },
    handleSubmit() {
      let validationResult = this.validationGroup.validate();
      if (validationResult.isValid) {
        this.editTeamAdmin(this.selectedLeagueId).then(() => {
          this.popupVisible = false;
          this.$emit("closed");
          useToast().success("Edycja drużyny zakończona pomyślnie!");
        });
      }
    },
  },
  mounted() {
    this.setLeaguesToLookup().then((response) => {
      this.leagues = response.data;
    });
    this.popupVisible = true;
    this.setTeamDetails(this.teamId);
  },
  components: {
    DxValidator,
    DxRequiredRule,
    DxValidationGroup,
    DxPopup,
    DxToolbarItem,
    DxSelectBox,
    DxButton,
    DxTextBox,
    DxCheckBox,
  },
  beforeUnmount() {
    this.RESET_TEAM_DETAILS();
  },
};
</script>
