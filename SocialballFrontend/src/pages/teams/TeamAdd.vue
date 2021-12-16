<template>
  <div class="big-data-grid">
    <h3>Wniosek o dodanie nowej drużyny</h3>
    <p>
      Ze względów bezpieczeństwa, każda nowa drużyna wymaga akceptacji przez
      administratora serwisu.
    </p>
    <form>
      <DxValidationGroup :ref="groupRefKey">
        <div class="primary-border">
          <h4 class="line">Dane drużyny</h4>
          <div class="row">
            <div class="col">
              <label for="nameTextBox" class="form-label">Nazwa</label>
              <DxTextBox v-model="Name" id="nameTextBox">
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
              <label for="imageUploader" class="form-label">Herb drużyny</label>
              <DxFileUploader
                select-button-text="Dodaj herb drużyny"
                label-text="zaleca się obraz kwadratowy"
                accept="image/*"
                upload-mode="useForm"
                id="imageUploader"
                readyToUploadMessage="Gotowe do przesłania"
                @value-changed="imageAdded"
              />
            </div>
          </div>
        </div>
        <div class="secondary-border">
          <h4 class="line">Dane do logowania zarządu</h4>
          <div class="row">
            <div class="col">
              <label for="loginUsernameTextBox" class="form-label"
                >Nazwa użytkownika</label
              >
              <DxTextBox v-model="LoginUsername" id="loginUsernameTextBox">
                <DxValidator>
                  <DxRequiredRule message="Nazwa użytkownika jest wymagana!" />
                  <DxAsyncRule
                    :validation-callback="validateUsername"
                    message="Taki użytkownik istnieje już w systemie!"
                  />
                </DxValidator>
              </DxTextBox>
            </div>
            <div class="col">
              <label for="emailTextBox" class="form-label">E-mail</label>
              <DxTextBox v-model="Email" id="emailTextBox">
                <DxValidator>
                  <DxEmailRule message="E-mail jest nieprawidłowy." />
                </DxValidator>
              </DxTextBox>
            </div>
          </div>
          <div class="row mt-4">
            <div class="col">
              <label for="loginPasswordTextBox" class="form-label">Hasło</label>
              <DxTextBox
                v-model="LoginPassword"
                id="loginPasswordTextBox"
                mode="password"
              >
                <DxValidator>
                  <DxRequiredRule message="Hasło jest wymagane!" />
                </DxValidator>
              </DxTextBox>
            </div>
            <div class="col">
              <label for="loginPasswordConfirmationTextBox" class="form-label"
                >Powtórz hasło</label
              >
              <DxTextBox id="loginPasswordConfirmationTextBox" mode="password">
                <DxValidator>
                  <DxRequiredRule message="Powtórzenie hasła jest wymagane!" />
                  <DxCompareRule
                    :comparison-target="passwordComparison"
                    message="Hasła się nie zgadzają!"
                  />
                </DxValidator>
              </DxTextBox>
            </div>
          </div>
          <div class="row mt-4">
            <div class="px-3">
              <vue-recaptcha
                v-show="true"
                siteKey="6LejOUwdAAAAAJYhE8VtA-J0PNrriXDlNyLg9ETw"
                size="normal"
                theme="dark"
                hl="pl"
                @verify="recaptchaVerified"
                @expire="recaptchaExpired"
                @fail="recaptchaFailed"
                ref="vueRecaptcha"
                :style="{ border: borderStyle }"
              >
              </vue-recaptcha>
              <DxValidator :adapter="recaptchaValidatorConfig">
                <DxRequiredRule message="Zaznacz pole 'Nie jestem robotem'." />
              </DxValidator>
            </div>
          </div>
        </div>
        <div class="row mt-4">
          <div class="col">
            <DxValidationSummary />
          </div>
          <div class="col text-right">
            <DxButton
              text="Wyślij wniosek"
              type="default"
              @click="handleSubmit"
              ref="submitButton"
            />
          </div>
        </div>
      </DxValidationGroup>
    </form>
  </div>
</template>
<script>
import DxTextBox from "devextreme-vue/text-box";
import DxSelectBox from "devextreme-vue/select-box";
import DxButton from "devextreme-vue/button";
import {
  DxValidator,
  DxRequiredRule,
  DxAsyncRule,
  DxEmailRule,
  DxCompareRule,
} from "devextreme-vue/validator";
import DxValidationGroup from "devextreme-vue/validation-group";
import DxValidationSummary from "devextreme-vue/validation-summary";

import { mapActions, mapMutations } from "vuex";
import { createHelpers } from "vuex-map-fields";
const { mapFields } = createHelpers({
  getterType: "teams/getField",
  mutationType: "teams/updateField",
});

import { useToast } from "vue-toastification";
import vueRecaptcha from "vue3-recaptcha2";
import { DxFileUploader } from "devextreme-vue/file-uploader";
import { userTypeEnum } from "../../enums/userTypeEnum";

export default {
  data() {
    const callbacks = [];
    const recaptchaValidatorConfig = {
      getValue: () => {
        return this.isRecaptchaVerified;
      },
      applyValidationResults: (e) => {
        this.borderStyle = e.isValid ? "none" : ".5px solid #813e3c";
      },
      validationRequestsCallbacks: callbacks,
    };
    return {
      leagues: {},
      groupRefKey: "targetGroup",
      isRecaptchaVerified: false,
      recaptchaValidatorConfig,
      borderStyle: "none",
      userTypeEnum,
    };
  },
  computed: {
    ...mapFields([
      "team.Name",
      "team.LeagueId",
      "team.Image",
      "team.LoginUsername",
      "team.Email",
      "team.LoginPassword",
    ]),
    validationGroup: function() {
      return this.$refs[this.groupRefKey].instance;
    },
  },
  methods: {
    ...mapActions({
      addTeam: "teams/addTeam",
      validateUsername: "players/validateUsername",
      setLeaguesToLookup: "teams/setLeaguesToLookup",
    }),
    ...mapMutations({
      RESET_TEAM_DETAILS: "teams/RESET_TEAM_DETAILS",
    }),
    passwordComparison() {
      return this.LoginPassword;
    },
    handleSubmit() {
      let validationResult = this.validationGroup.validate();
      const buttonInstance = this.$refs["submitButton"].instance;
      if (validationResult.isValid && this.isRecaptchaVerified) {
        validationResult.status === "pending" &&
          validationResult.complete.then((res) => {
            if (res.isValid) {
              buttonInstance.option("icon", "fas fa-circle-notch fa-spin");
              buttonInstance.option("text", "Dodaję wniosek...");
              this.addTeam().then(() => {
                buttonInstance.option("icon", "");
                buttonInstance.option("text", this.getSubmitText());
                useToast().success(
                  "Wniosek o dodanie drużyny został wysłany pomyślnie!"
                );
                this.$router.push({ path: "/" });
              });
            }
          });
      }
    },
    recaptchaVerified() {
      this.isRecaptchaVerified = true;
    },
    recaptchaExpired() {
      this.isRecaptchaVerified = false;
      this.$refs.vueRecaptcha.reset();
    },
    recaptchaFailed() {
      this.isRecaptchaVerified = false;
    },
    imageAdded(e) {
      var reader = new FileReader();
      var fileByteArray = [];
      reader.readAsArrayBuffer(e.value[0]);
      reader.onloadend = function(evt) {
        if (evt.target.readyState == FileReader.DONE) {
          var arrayBuffer = evt.target.result,
            array = new Uint8Array(arrayBuffer);
          for (var i = 0; i < array.length; i++) {
            fileByteArray.push(array[i]);
          }
        }
      };
      this.Image = fileByteArray;
    },
  },
  mounted() {
    this.setLeaguesToLookup().then((response) => {
      this.leagues = response.data;
    });
    this.RESET_TEAM_DETAILS();
  },
  components: {
    DxTextBox,
    DxSelectBox,
    DxButton,
    DxValidator,
    DxRequiredRule,
    DxAsyncRule,
    DxEmailRule,
    DxCompareRule,
    DxValidationGroup,
    DxValidationSummary,
    vueRecaptcha,
    DxFileUploader,
  },
  beforeUnmount() {
    this.RESET_TEAM_DETAILS();
  },
};
</script>
