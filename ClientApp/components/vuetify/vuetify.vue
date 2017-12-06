<template>
  <div>
    <h1>vuetify</h1>
    <v-dialog v-model="dialog" lazy absolute>
      <v-btn primary dark slot="activator">Open Dialog</v-btn>
      <v-card>
        <v-card-title>
          <div class="headline">Use Google's location service?</div>
        </v-card-title>
        <v-card-text>Let Google help apps determine location. This means sending anonymous location data to Google, even when no apps are running.</v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn class="green--text darken-1" flat="flat" @click.native="dialog = false">Disagree</v-btn>
          <v-btn class="green--text darken-1" flat="flat" @click.native="dialog = false">Agree</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-text-field name="input-10-1" label="Enter your password" hint="At least 8 characters" v-model="password" min="8" :append-icon="e1 ? 'visibility' : 'visibility_off'" :append-icon-cb="() => (e1 = !e1)" :type="e1 ? 'password' : 'text'" counter></v-text-field>
    <div style="width:100%; height:200px; background-color:red;">
    </div>
  </div>
</template>

<script src='./vuetify.ts'></script>
