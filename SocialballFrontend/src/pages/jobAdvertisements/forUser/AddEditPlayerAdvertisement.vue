<template>
  <div>
    <h3>Dodaj/edytuj własne ogłoszenie</h3>
    <form>
      <DxValidationGroup :ref="groupRefKey">
        <div class="row">
          <div class="col">
            <label for="locationTextBox" class="form-label"
              >Preferowana lokalizacja</label
            >
            <DxTextBox v-model="Location" id="locationTextBox" />
          </div>
          <div class="col">
            <label for="earningsPerMonthNumberBox" class="form-label"
              >Preferowane zarobki [PLN/miesiąc]</label
            >
            <DxNumberBox v-model="Earnings" id="earningsPerMonthNumberBox" />
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
              :max-height="90"
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
          <div class="col text-right">
            <DxButton text="Zapisz" type="default" @click="handleSubmit" />
          </div>
        </div>
      </DxValidationGroup>
    </form>
  </div>
</template>
<script>
import DxTextBox from "devextreme-vue/text-box";
import DxTextArea from "devextreme-vue/text-area";
import DxButton from "devextreme-vue/button";
import { DxCheckBox } from "devextreme-vue/check-box";
import { DxValidator, DxRequiredRule } from "devextreme-vue/validator";
import DxValidationGroup from "devextreme-vue/validation-group";
import DxValidationSummary from "devextreme-vue/validation-summary";
import { DxNumberBox } from "devextreme-vue/number-box";

import { mapActions, mapMutations, mapGetters } from "vuex";
import { createHelpers } from "vuex-map-fields";
const { mapFields } = createHelpers({
  getterType: "jobAdvertisements/getField",
  mutationType: "jobAdvertisements/updateField",
});

import { useToast } from "vue-toastification";

export default {
  name: "AddEditPlayerAdvertisement",
  components: {
    DxTextBox,
    DxTextArea,
    DxButton,
    DxValidator,
    DxRequiredRule,
    DxValidationGroup,
    DxValidationSummary,
    DxCheckBox,
    DxNumberBox,
  },
  data() {
    return {
      groupRefKey: "targetGroup",
    };
  },
  computed: {
    ...mapFields([
      "jobAdvertisement.Location",
      "jobAdvertisement.Content",
      "jobAdvertisement.Earnings",
      "jobAdvertisement.UserId",
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
      updateUserJobAdvertisement:
        "jobAdvertisements/updateUserJobAdvertisement",
      setUserJobAdvertisementDetails:
        "jobAdvertisements/setUserJobAdvertisementDetails",
    }),
    ...mapMutations({
      RESET_JOB_ADVERTISEMENT_FORM:
        "jobAdvertisements/RESET_JOB_ADVERTISEMENT_FORM",
    }),
    handleSubmit() {
      let validationResult = this.validationGroup.validate();
      if (validationResult.isValid) {
        this.updateUserJobAdvertisement().then(() => {
          useToast().success("Zmiany w ogłoszeniu zostały zapisane pomyślnie!");
        });
      }
    },
  },
  mounted() {
    this.UserId = this.getLoggedInUser.id;
    this.setUserJobAdvertisementDetails(this.getLoggedInUser.id);
  },
  beforeUnmount() {
    this.RESET_JOB_ADVERTISEMENT_FORM();
  },
};
</script>
