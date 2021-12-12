<template>
  <DxPopover
    :width="300"
    :visible="loginPopoverVisible"
    :shading="true"
    target="#showLoginButton"
    position="top"
    shading-color="rgba(0, 0, 0, 0.3)"
    @hiding="onHiding"
    ref="loginPopover"
  >
    <div class="popover-container text-center">
      <h3 class="m-4">Logowanie</h3>
      <form>
        <DxValidationGroup :ref="groupRefKey">
          <div class="row m-3">
            <div class="col">
              <label for="loginTextBox">Login</label>
              <DxTextBox v-model="Username" id="loginTextBox">
                <DxValidator>
                  <DxRequiredRule message="Login jest wymagany!" />
                </DxValidator>
              </DxTextBox>
            </div>
          </div>
          <div class="row m-3">
            <div class="col">
              <label for="passwordTextBox">Hasło</label>
              <DxTextBox
                v-model="Password"
                id="passwordTextBox"
                mode="password"
              >
                <DxValidator>
                  <DxRequiredRule message="Hasło jest wymagane!" />
                </DxValidator>
              </DxTextBox>
            </div>
          </div>
          <div class="row mt-5">
            <div class="col">
              <DxButton
                text="Zaloguj się"
                type="default"
                @click="handleSubmit"
                ref="submitButton"
              />
            </div>
          </div>
        </DxValidationGroup>
      </form>
    </div>
  </DxPopover>
</template>
<script>
import DxTextBox from "devextreme-vue/text-box";
import DxButton from "devextreme-vue/button";
import DxValidationGroup from "devextreme-vue/validation-group";
import { mapActions } from "vuex";
import { DxValidator, DxRequiredRule } from "devextreme-vue/validator";
import { DxPopover } from "devextreme-vue/popover";
import { useToast } from "vue-toastification";

import { createHelpers } from "vuex-map-fields";
const { mapFields } = createHelpers({
  getterType: "authentication/getField",
  mutationType: "authentication/updateField",
});

export default {
  name: "LoginPlayer",
  props: {
    loginPopoverVisible: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      groupRefKey: "targetGroup",
    };
  },
  computed: {
    ...mapFields(["form.Username", "form.Password"]),
    validationGroup: function() {
      return this.$refs[this.groupRefKey].instance;
    },
  },
  methods: {
    ...mapActions({
      login: "authentication/login",
    }),
    async handleSubmit() {
      let validationResult = this.validationGroup.validate();
      if (validationResult.isValid) {
        const buttonInstance = this.$refs["submitButton"].instance;
        buttonInstance.option("icon", "fas fa-circle-notch fa-spin");
        buttonInstance.option("text", "Loguję");
        let result = await this.login();
        buttonInstance.option("icon", "");
        buttonInstance.option("text", "Zaloguj się");
        if (result.ClassName === "System.Exception") {
          useToast().error("Logowanie nieudane!");
        } else if (result.token) {
          this.hideLoginPopover();
          this.$router.push({ path: "/profile" });
          useToast().success("Zalogowano pomyślnie!");
        }
      }
    },
    hideLoginPopover() {
      this.$refs.loginPopover.instance.hide();
    },
    onHiding() {
      this.$emit("hideLoginForm");
    },
  },
  components: {
    DxPopover,
    DxTextBox,
    DxButton,
    DxValidationGroup,
    DxValidator,
    DxRequiredRule,
  },
};
</script>
