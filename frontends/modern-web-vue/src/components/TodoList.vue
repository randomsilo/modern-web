<template>
  <b-container>

    <!-- alert box -->
    <b-row class="justify-content-md-center">
      <b-alert
        :show="alertDismissCountDown"
        :class="{ alertCssClass }" 
        dismissible
        @dismissed="alertDismissCountDown=0"
        @dismiss-count-down="alertCountDownChanged">
        <p>{{ alertMessage }}</p>
        <b-progress
          :class="{ alertCssClass }"
          :max="alertDismissSecs"
          :value="alertDismissCountDown"
          height="4px">
        </b-progress>
      </b-alert>
    </b-row>

    <!-- loading spinner -->
    <b-row class="justify-content-md-center" v-if="isLoading()">
      <b-spinner variant="primary" label="Spinning"></b-spinner>
    </b-row>

    <!-- title bar -->
    <b-row class="justify-content-md-center">
      <b-col cols="9">
        <h2>{{ title }} </h2>
      </b-col>

      <!-- table specific controls -->
      <b-col cols="2" class="text-right">
        <b-button-group>
          <b-button
            v-if="showTable()"
            class="btn btn-info"
            size="sm">
            Rows {{ rows }}
          </b-button>
          <b-button
            v-if="showTable()"
            class="btn btn-warning"
            size="sm"
            @click="getListing()">
            <b-icon-arrow-repeat></b-icon-arrow-repeat> 
          </b-button>
        </b-button-group>
      </b-col>

      <!-- view toggle buttons -->
      <b-col cols="1" class="text-right">
        <b-button-group>
          <b-button 
            v-if="showTable()"
            class="btn btn-success"
            size="sm"
            @click="toggleTableVisible()">
            <b-icon-plus></b-icon-plus> 
          </b-button>
          <b-button
            v-if="showForm()" 
            class="btn btn-success"
            size="sm"
            @click="toggleTableVisible()">
            <b-icon-table></b-icon-table> 
          </b-button>
        </b-button-group>
      </b-col>
    </b-row>

    <!-- table listing -->
    <b-row class="justify-content-md-center" v-if="showTable()">
      <b-table
        id="listing"
        :items="items"
        :fields="fields"
        :per-page="perPage"
        :current-page="currentPage"
        small
      ></b-table>

      <b-pagination
        v-model="currentPage"
        :total-rows="rows"
        :per-page="perPage"
        aria-controls="listing"
      ></b-pagination>
    </b-row>

    <!-- form -->
    <b-row v-if="showForm()">
      <b-form @submit="onSubmit" @reset="onReset" v-if="formVisible">
        <b-form-group
          id="input-group-1"
          label="Email address:"
          label-for="input-1"
          description="We'll never share your email with anyone else.">
          <b-form-input
            id="input-1"
            v-model="form.email"
            type="email"
            required
            placeholder="Enter email"
          ></b-form-input>
        </b-form-group>

        <b-form-group id="input-group-2" label="Your Name:" label-for="input-2">
          <b-form-input
            id="input-2"
            v-model="form.name"
            required
            placeholder="Enter name"
          ></b-form-input>
        </b-form-group>

        <b-form-group id="input-group-3" label="Food:" label-for="input-3">
          <b-form-select
            id="input-3"
            v-model="form.food"
            :options="foods"
            required
          ></b-form-select>
        </b-form-group>

        <b-form-group id="input-group-4">
          <b-form-checkbox-group v-model="form.checked" id="checkboxes-4">
            <b-form-checkbox value="me">Check me out</b-form-checkbox>
            <b-form-checkbox value="that">Check that out</b-form-checkbox>
          </b-form-checkbox-group>
        </b-form-group>

        <b-button type="submit" variant="primary">Submit</b-button>
        <b-button type="reset" variant="danger">Reset</b-button>
      </b-form>
    </b-row>

    <!-- form debug -->
    <b-row v-if="showForm()">
      <b-card class="mt-3" header="Form Data Result">
        <pre class="m-0">{{ form }}</pre>
      </b-card>
    </b-row>
  </b-container>
</template>

<script>
/* eslint-disable */
export default {
  name: 'TodoList'
  , mounted() {
    this.getListing();
  }

  , data() {

    return {
      tableVisible: true,
      tableLoading: false,
      alert: false,
      alertMessage: "",
      alertCssClass: "info",
      alertDismissSecs: 5,
      alertDismissCountDown: 0,
      title: 'Todo List',
      perPage: 20,
      currentPage: 1,
      fields: [
        { key: 'todoEntryId', label: 'ID' },
        { key: 'summary', label: 'Summary' },
        { key: 'details', label: 'Details' }
      ],
      items: [
      ],
      form: {
        email: '',
        name: '',
        food: null,
        checked: []
      },
      foods: [{ text: 'Select One', value: null }, 'Carrots', 'Beans', 'Tomatoes', 'Corn'],
      formVisible: true
    }
  }

  , computed: {
    rows() {
      return this.items.length
    }
  }

  , methods: {
    getListing() {
      this.tableLoading = true;

      this.TodoListApi.get('/TodoEntry')
        .then(request => this.handleListingResponse(request))
        .catch(() => this.handleListingError())
    }

    , toggleTableVisible() {
      this.tableVisible = !this.tableVisible;
      if (this.tableVisible) {
        this.title = "Todo Listing";
      } else {
        this.title = "Todo: Add New";
      }
    }

    , showTable() {
      return this.tableVisible;
    }

    , showForm() {
      return !this.tableVisible;
    }

    , isLoading() {
      return this.tableLoading;
    }

    , setAlert(message, cssClass) {
      if (message == false) {
        this.alertDismissCountDown = 0;
        this.alertMessage = "";
      } else {
        this.alertDismissCountDown = this.alertDismissSecs;
        this.alertMessage = message;
      }

      this.alertCssClass = "info";
      if (cssClass != null && cssClass.length > 0) {
        this.alertCssClass = cssClass;
      }
    }

    , alertCountDownChanged(dismissCountDown) {
      this.alertDismissCountDown = dismissCountDown;
    }

    , handleListingResponse (request) {
      this.items = request.data;

      if (this.items.length == 0) {
        this.setAlert("No data found.", "info");
        this.tableVisible = false;
      }
      else {
        this.setAlert("Loaded " + this.rows + " records", "success");
        this.tableVisible = true;
      }

      this.tableLoading = false;
    }

    , handleListingError() {
      this.tableLoading = false;
      this.setAlert("An error has occurred.", "danger");
    }

    , onSubmit(evt) {
      evt.preventDefault()
      alert(JSON.stringify(this.form))
    }
    
    , onReset(evt) {
      evt.preventDefault();

      // Reset our form values
      this.form.email = '';
      this.form.name = '';
      this.form.food = null;
      this.form.checked = [];

      // Trick to reset/clear native browser form validation state
      this.formVisible = false;
      this.$nextTick(() => {
        this.formVisible = true;
      });

    }
    

  }
}
</script>

<style lang="css">

</style>