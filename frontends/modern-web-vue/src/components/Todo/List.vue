<template>
  <div class="container-fluid h-100">

    <div class="row">
      <h2>Todo</h2>
    </div>

    <!-- alert box -->
    <b-row class="justify-content-md-center">
      <b-alert
        :show="alertDismissCountDown"
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
    <div class="row">
      <div class="card col-12">
        <div class="card-header">
          <ul class="nav nav-tabs card-header-tabs">
            <li class="nav-item">
              <a class="nav-link" href="#" :class="showTable() ? 'active' : ''" @click="toggleTableVisible()">Listing</a>
            </li>
            <li class="nav-item" >
              <a class="nav-link" href="#" :class="showForm() ? 'active' : ''" @click="toggleTableVisible()">Form</a>
            </li>
          </ul>
        </div>
        
        <div class="card-body">
            
          <!-- table listing -->
          <div class="row" v-if="showTable()">
            <div class="col-12">
              <button class="btn btn-primary btn-sm float-right" @click="getListing()">Refresh</button>
            </div>

            <b-table
              id="listing"
              :items="items"
              :fields="fields"
              :per-page="perPage"
              :current-page="currentPage"
              small></b-table>

            <b-pagination
              v-model="currentPage"
              :total-rows="rows"
              :per-page="perPage"
              aria-controls="listing"></b-pagination>

          </div>

          <!-- form -->
          <div class="row" v-if="showForm()">
            <form class="col-12" @submit.prevent="onSave">

              <div class="form-group row">
                <label for="todoEntryId" class="col-3">ID</label>
                <input v-model="form.todoEntryId" type="text" id="todoEntryId" class="form-control form-control-sm col" autocomplete="off" readonly>
              </div>

              <div class="form-group row">
                <label for="summary" class="col-3">Summary</label>
                <input v-model="form.summary" type="text" id="summary" class="form-control form-control-sm col" autocomplete="off" required autofocus>
              </div>

              <div class="form-group row">
                <label for="details" class="col-3">Details</label>
                <input v-model="form.details" type="text" id="details" class="form-control form-control-sm col" autocomplete="off">
              </div>

              <div class="form-group row">
                <div class="col-12">
                  <button type="reset" class="btn btn-danger btn-sm float-right" @click="onReset">Clear</button>
                  <button type="submit" class="btn btn-primary btn-sm float-right">Save</button>
                </div>
              </div>

            </form>
          </div>


        </div>
      </div>
    </div>



    <div class="row">
      <br />
      <br />
    </div>

    

    

    <!-- form debug -->
    <b-row v-if="showForm()">
      <b-card class="mt-3" header="Form Data Result">
        <pre class="m-0">{{ form }}</pre>
      </b-card>
    </b-row>
  </div>
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
      alertCssClass: "secondary",
      alertDismissSecs: 5,
      alertDismissCountDown: 0,
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
        todoEntryId: '',
        summary: '',
        details: null
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

      this.alertCssClass = "secondary";
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
        this.setAlert("No data found.", "warning");
        this.tableVisible = false;
      }
      else {
        this.setAlert("Loaded " + this.rows + " records", "secondary");
        this.tableVisible = true;
      }

      this.tableLoading = false;
    }

    , handleListingError() {
      this.tableLoading = false;
      this.setAlert("An error has occurred.", "danger");
    }

    , onSave(evt) {
      debugger;

      evt.preventDefault();

      if (this.form.todoEntryId == '') {
        // add
        this.TodoListApi.post('/TodoEntry', 
        {
          "todoEntryId": null,
          "summary": this.form.summary,
          "details": this.form.details,
          "dueDate": null,
          "entryStatusIdRef": null
        })
          .then(request => this.onSaveSuccess(request))
          .catch(() => this.onSaveFail());
      }
      else {
        // edit

      }
    }

    , onSaveSuccess(req) {
      debugger;
      this.setAlert("Added!", "success");
    }

    , onSaveFail() {
      debugger;
      this.setAlert("An error has occurred.", "danger");
    }
    
    , onReset(evt) {
      evt.preventDefault();

      // Reset our form values
      this.form.todoEntryId = '';
      this.form.summary = '';
      this.form.details = '';

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