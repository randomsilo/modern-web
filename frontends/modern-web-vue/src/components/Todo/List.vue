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
              <a class="nav-link" href="#" :class="isTableVisible() ? 'active' : ''" @click="showTable()">Listing</a>
            </li>
            <li class="nav-item" >
              <a class="nav-link" href="#" :class="isFormVisible() ? 'active' : ''" @click="showForm()">Form</a>
            </li>
          </ul>
        </div>
        
        <div class="card-body">
            
          <!-- table listing -->
          <div class="row" v-if="isTableVisible()">
            <table class="table">
              <thead class="thead-light">
                <tr>
                  <th scope="col" colspan="6">
                    <button class="btn btn-success btn-sm float-right" title="add new" @click="addForm()"><b-icon icon="plus"></b-icon></button>
                    <button class="btn btn-primary btn-sm float-right" title="refresh listing" @click="onGetListing(true)"><b-icon icon="arrow-repeat"></b-icon></button>
                  </th>
                </tr>
              </thead>
              <thead class="thead-dark">
                <tr>
                  <th scope="col">Action</th>
                  <th scope="col">ID</th>
                  <th scope="col">Summary</th>
                  <th scope="col">Details</th>
                  <th scope="col">Due Date</th>
                  <th scope="col">Status</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for='(item, index) in this.items' :key='item.todoEntryId'>
                  <th scope="row">
                    <button class="btn btn-danger btn-sm float-right" title="remove" @click="onRemove(index)"><b-icon icon="trash"></b-icon></button>
                    <button class="btn btn-secondary btn-sm float-right" title="edit" @click="onEdit(index)"><b-icon icon="pencil"></b-icon></button>
                  </th>
                  <td>{{ item.todoEntryId }}</td>
                  <td>{{ item.summary }}</td>
                  <td>{{ item.details }}</td>
                  <td>{{ translateDateTime(item.dueDate) }}</td>
                  <td>{{ translateEntryStatusId(item.entryStatusIdRef) }}</td>
                </tr>
              </tbody>
            </table>
          </div>

          <!-- form -->
          <div class="row" v-if="isFormVisible()">
            <form class="col col-lg-6 col-md-10 col-sm-12" @submit.prevent="onSave">

              <label for="todoEntryId">ID</label>
              <input v-model="form.todoEntryId" type="text" id="todoEntryId" class="form-control form-control-sm col" autocomplete="off" readonly>
              <br />
              
              <label for="summary">Summary</label>
              <input v-model="form.summary" type="text" id="summary" class="form-control form-control-sm col" autocomplete="off" required autofocus>
              <br />

              <label for="details">Details</label>
              <input v-model="form.details" type="text" id="details" class="form-control form-control-sm col" autocomplete="off">
              <br />

              <label for="dueDate">Due Date</label>
              <input v-model="form.dueDate" type="datetime-local" id="dueDate" class="form-control form-control-sm col" autocomplete="off">
              <br />

              <label for="entryStatusIdRef">Entry Status</label>
              <select class="form-control col" id="entryStatusIdRef" v-model="form.entryStatusIdRef">
                <option v-for="item in entryStatuses" v-bind:key="item.todoStatusId" v-bind:value="item.todoStatusId">{{ item.choiceName }}</option>
              </select>
              <br />

              <div class="form-group row">
                <div class="col-12">
                  <button type="reset" class="btn btn-warning btn-sm float-right" title="clear" @click="onReset"><b-icon icon="x-square"></b-icon></button>
                  <button type="submit" class="btn btn-primary btn-sm float-right">Save</button>
                </div>
              </div>
              <br />
              <br />

            </form>
          </div>


        </div>
      </div>
    </div>


    <!-- form debug -->
    <!--
    <b-row v-if="isFormVisible()">
      <br />
      <br />
      <b-card class="mt-3" header="Form Data Result">
        <pre class="m-0">{{ form }}</pre>
      </b-card>
    </b-row>
    -->

  </div>
</template>

<script>
/* eslint-disable */
export default {
  name: 'TodoList'
  , mounted() {
    this.onGetEntryStatuses();
    this.onGetListing(true);
  }

  , data() {

    return {
      tableRefreshNeeded: false,
      tableVisible: true,
      tableLoading: false,
      alert: false,
      alertMessage: "",
      alertCssClass: "secondary",
      alertDismissSecs: 5,
      alertDismissCountDown: 0,
      items: [
      ],
      form: {
        todoEntryId: '',
        summary: '',
        details: null,
        dueDate: null,
        entryStatusIdRef: null
      },
      entryStatuses: [],
      formVisible: true
    }
  }

  , computed: {
    rows() {
      return this.items.length
    }
  }

  , methods: {
    toggleTableVisible() {
      this.tableVisible = !this.tableVisible;
    }

    , showTable() {
      this.tableVisible = true;

      if (this.tableRefreshNeeded) {
        this.onGetListing(false);
      }
    }

    , showForm() {
      this.tableVisible = false;
    }

    , addForm() {
      this.onClearForm();
      this.tableVisible = false;
    }

    , isTableVisible() {
      return this.tableVisible;
    }

    , isFormVisible() {
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

    , onClearForm() {
      this.form.todoEntryId = '';
      this.form.summary = '';
      this.form.details = '';
      this.form.dueDate = null;
      this.form.entryStatusIdRef = null;
    }

    , onGetEntryStatuses() {
      this.TodoListApi.get('/TodoStatus')
        .then(request => this.onGetEntryStatusesSuccess(request))
        .catch(() => this.onGetEntryStatusesFail())
    }

    , onGetEntryStatusesSuccess(request) {
      this.entryStatuses = request.data;
    }

    , onGetEntryStatusesFail() {
      this.setAlert("An error has occurred.  Unable to get entry statuses.", "danger");
    }

    , translateEntryStatusId(id) {
      var descr = "";

      if (id != null) {
        for (let item of this.entryStatuses) {
          if (item.todoStatusId == id) {
            descr = item.choiceName;
            break;
          }
        }
      }

      return descr;
    }

    , translateDateTime(dateValue) {
      var dateFomatted = "";

      if (dateValue != null) {
        var dateObj = new Date(dateValue);
        if (dateObj != null && dateObj instanceof Date) {
          var hours = dateObj.getHours();
          var ampm = hours >= 12 ? 'PM' : 'AM';
          hours = hours % 12;
          hours = hours ? hours : 12; // the hour '0' should be '12'

          dateFomatted = 
                  ("00" + (dateObj.getMonth() + 1)).slice(-2) 
                  + "/" + ("00" + dateObj.getDate()).slice(-2) 
                  + "/" + dateObj.getFullYear() + " " 
                  + ("00" + hours).slice(-2) + ":" 
                  + ("00" + dateObj.getMinutes()).slice(-2) 
                  + ":" + ("00" + dateObj.getSeconds()).slice(-2)
                  + " "
                  + ampm;
        }
      }

      return dateFomatted
    }

    , onGetListing(showStatusAlert) {
      this.tableLoading = true;

      this.TodoListApi.get('/TodoEntry')
        .then(request => this.onGetListingSuccess(request, showStatusAlert))
        .catch(() => this.onGetListingFail())
    }

    , onGetListingSuccess (request, showStatusAlert) {
      this.items = request.data;

      if (this.items.length == 0) {
        if (showStatusAlert) {
          this.setAlert("No data found.", "warning");
        }
        this.tableVisible = false;
      }
      else {
        if (showStatusAlert) {
          this.setAlert("Loaded " + this.rows + " records", "secondary");
        }
        this.tableVisible = true;
      }

      this.tableLoading = false;
      this.tableRefreshNeeded = false;
    }

    , onGetListingFail() {
      this.tableLoading = false;
      this.setAlert("An error has occurred.", "danger");
    }

    , onSave(evt) {
      evt.preventDefault();

      if (this.form.todoEntryId == '') {
        // add
        this.TodoListApi.post('/TodoEntry', 
        {
          "todoEntryId": null,
          "summary": this.form.summary,
          "details": this.form.details,
          "dueDate": new Date(this.form.dueDate).toJSON(),
          "entryStatusIdRef": this.form.entryStatusIdRef
        })
          .then(request => this.onSaveSuccess(request))
          .catch(() => this.onSaveFail());
      }
      else {
        // edit
        this.TodoListApi.put('/TodoEntry/'+this.form.todoEntryId, 
        {
          "todoEntryId": this.form.todoEntryId,
          "summary": this.form.summary,
          "details": this.form.details,
          "dueDate": new Date(this.form.dueDate).toJSON(),
          "entryStatusIdRef": this.form.entryStatusIdRef
        })
          .then(request => this.onSaveSuccess(request))
          .catch(() => this.onSaveFail());
      }
    }

    , onSaveSuccess(request) {
      this.setAlert("Saved!", "success");
      this.tableRefreshNeeded = true;
    }

    , onSaveFail() {
      this.setAlert("An error has occurred.", "danger");
    }
    
    , onReset(evt) {
      evt.preventDefault();

      // Reset our form values
      this.onClearForm();

      // Trick to reset/clear native browser form validation state
      this.formVisible = false;
      this.$nextTick(() => {
        this.formVisible = true;
      });

    }

    , onEdit(idx) {
      var item = this.items[idx];
      if (item) {
        // Set form values
        this.form.todoEntryId = item.todoEntryId;
        this.form.summary = item.summary;
        this.form.details = item.details;
        this.form.dueDate = item.dueDate;
        this.form.entryStatusIdRef = item.entryStatusIdRef;

        this.showForm();
      }
      else {
        this.setAlert("An error has occurred.", "danger");
      }
    }

    , onRemove(idx) {
      var item = this.items[idx];
      if (item) {
        this.TodoListApi.delete('/TodoEntry/'+item.todoEntryId)
          .then(request => this.onRemoveSuccess(request))
          .catch(() => this.onRemoveFail());
      }
      else {
        this.setAlert("An error has occurred.", "danger");
      }
    }

    , onRemoveSuccess(req) {
      this.onClearForm();
      this.onGetListing(false);
      this.setAlert("Removed!", "success");
    }

    , onRemoveFail() {
      this.setAlert("An error has occurred.", "danger");
    }
    
  }
}
</script>

<style lang="css">

</style>