<template>
  <div>
    <DxPopup
      v-model:visible="popupVisible"
      :drag-enabled="false"
      :close-on-outside-click="false"
      :show-close-button="false"
      :show-title="true"
      :width="500"
      :height="580"
      container=".dx-viewport"
      :title="getTitle()"
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
              <label for="locationTextBox" class="form-label"
                >Lokalizacja</label
              >
              <DxTextBox v-model="Location" id="locationTextBox">
                <DxValidator>
                  <DxRequiredRule
                    message="Określenie lokalizacji jest wymagane!"
                  />
                </DxValidator>
              </DxTextBox>
            </div>
            <div class="col">
              <label for="trainingSessionsPerWeekNumberBox" class="form-label"
                >Liczba sesji treningowych/tydzień</label
              >
              <DxNumberBox
                v-model="TrainingSessionsPerWeek"
                id="trainingSessionsPerWeekNumberBox"
              >
                <DxValidator>
                  <DxRequiredRule
                    message="Należy podać liczbę treningów/tydzień!"
                  />
                </DxValidator>
              </DxNumberBox>
            </div>
          </div>
          <div class="row mt-3">
            <div class="col">
              <label for="earningsPerMonthNumberBox" class="form-label"
                >Proponowane zarobki [PLN/miesiąc]</label
              >
              <DxNumberBox v-model="Earnings" id="earningsPerMonthNumberBox" />
            </div>
          </div>
          <div class="row mt-3">
            <div class="col">
              <label for="positionSelectBox" class="form-label">Pozycja</label>
              <DxSelectBox
                :dataSource="positions"
                value-expr="value"
                display-expr="name"
                v-model="Position"
                id="positionSelectBox"
              >
                <DxValidator>
                  <DxRequiredRule message="Wybór pozycji jest wymagany!" />
                </DxValidator>
              </DxSelectBox>
            </div>
          </div>
          <div class="row mt-3">
            <div class="col">
              <label for="contentTextBox" class="form-label"
                >Treść ogłoszenia</label
              >
              <DxTextArea
                v-model="Content"
                id="contentTextBox"
                :auto-resize-enabled="true"
                :height="50"
                :max-height="100"
              >
                <DxValidator>
                  <DxRequiredRule message="Treść ogłoszenia jest wymagana!" />
                </DxValidator>
              </DxTextArea>
            </div>
          </div>
          <div class="row mt-3">
            <div class="col">
              <DxCheckBox v-model:value="IsActive" id="isActiveCheckBox" />
              <label for="isActiveCheckBox" class="form-label ml-2"
                >Czy aktywne?</label
              >
            </div>
          </div>
          <div class="row mt-4">
            <div class="col">
              <DxValidationSummary />
            </div>
          </div>
        </DxValidationGroup>
      </form>
    </DxPopup>
  </div>
</template>
<script>
import DxTextBox from "devextreme-vue/text-box";
import { DxNumberBox } from "devextreme-vue/number-box";
import DxTextArea from "devextreme-vue/text-area";
import { DxCheckBox } from "devextreme-vue/check-box";
import { DxValidator, DxRequiredRule } from "devextreme-vue/validator";
import DxValidationGroup from "devextreme-vue/validation-group";
import DxValidationSummary from "devextreme-vue/validation-summary";
import { DxPopup, DxToolbarItem } from "devextreme-vue/popup";
import DxSelectBox from "devextreme-vue/select-box";

import { mapActions, mapMutations, mapGetters } from "vuex";
import { createHelpers } from "vuex-map-fields";
const { mapFields } = createHelpers({
  getterType: "jobAdvertisements/getField",
  mutationType: "jobAdvertisements/updateField",
});

import { useToast } from "vue-toastification";

export default {
  name: "AddEditMyTeamAdvertisement",
  props: {
    showToEdit: {
      type: Boolean,
      default: false,
    },
    jobAdvertisementId: {
      type: String,
      required: false,
    },
  },
  data() {
    return {
      popupVisible: false,
      positions: [],
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
    ...mapFields([
      "jobAdvertisement.Location",
      "jobAdvertisement.Content",
      "jobAdvertisement.TeamId",
      "jobAdvertisement.Earnings",
      "jobAdvertisement.TrainingSessionsPerWeek",
      "jobAdvertisement.Position",
      "jobAdvertisement.IsActive",
    ]),
    ...mapGetters({
      getLoggedInUser: "authentication/getLoggedInUser",
    }),
    validationGroup: function() {
      return this.$refs[this.groupRefKey].instance;
    },
  },
  methods: {
    ...mapActions({
      setJobAdvertisementDetails:
        "jobAdvertisements/setJobAdvertisementDetails",
      setPositionsToLookup: "jobAdvertisements/setPositionsToLookup",
      addMyTeamJobAdvertisement: "jobAdvertisements/addMyTeamJobAdvertisement",
      editMyTeamJobAdvertisement:
        "jobAdvertisements/editMyTeamJobAdvertisement",
      getUserTeamId: "authentication/getUserTeamId",
    }),
    ...mapMutations({
      RESET_JOB_ADVERTISEMENT_FORM:
        "jobAdvertisements/RESET_JOB_ADVERTISEMENT_FORM",
    }),
    getTitle() {
      return (
        (this.showToEdit ? "Edytuj" : "Dodaj") + " ogłoszenie swojej drużyny"
      );
    },
    handleSubmit() {
      let validationResult = this.validationGroup.validate();
      if (validationResult.isValid && this.showToEdit) {
        this.editMyTeamJobAdvertisement().then(() => {
          this.popupVisible = false;
          this.$emit("closed");
          useToast().success("Edycja ogłoszenia zakończona pomyślnie!");
        });
      } else if (validationResult.isValid && !this.showToEdit) {
        this.addMyTeamJobAdvertisement().then(() => {
          this.popupVisible = false;
          this.$emit("closed");
          useToast().success("Dodawanie ogłoszenia zakończone pomyślnie!");
        });
      }
    },
  },
  mounted() {
    this.setPositionsToLookup().then((response) => {
      this.positions = response.data;
    });
    this.popupVisible = true;
    this.getUserTeamId().then((response) => {
      this.TeamId = response.data;
    });
    if (this.showToEdit) {
      this.setJobAdvertisementDetails(this.jobAdvertisementId);
    }
  },
  components: {
    DxTextBox,
    DxTextArea,
    DxValidator,
    DxRequiredRule,
    DxValidationGroup,
    DxValidationSummary,
    DxCheckBox,
    DxPopup,
    DxToolbarItem,
    DxNumberBox,
    DxSelectBox,
  },
  beforeUnmount() {
    this.RESET_JOB_ADVERTISEMENT_FORM();
  },
};
</script>
