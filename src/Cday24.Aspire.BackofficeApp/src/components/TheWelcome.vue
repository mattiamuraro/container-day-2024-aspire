<script lang="ts">
interface TicketResponse {
  id: string
  title: string
  description: string
  aiExplanation: string
  creator: string
  creationDate: Date
  weight: number
};

type Tickets = TicketResponse[];

export default {
  name: 'TheWelcome',
  data() {
    return {
      tickets: [],
      loading: true,
      error: null,
      dialogMessage: ''
    }
  },
  mounted() {
    fetch('api/ticket')
      .then(response => response.json())
      .then(data => {
        this.tickets = data
      })
      .catch(error => {
        this.error = error
      })
      .finally(() => (this.loading = false))
  },
  methods: {
    showDialog(message: string) {
      this.dialogMessage = message;
      this.$refs.aiDialog.showModal();
    },
    closeDialog() {
      this.$refs.aiDialog.close();
    }
  }
}

</script>

<template>
  <table>
    <thead>
      <tr>
        <th>Title</th>
        <th>Description</th>
        <th>Customer</th>
        <th>CreationDate</th>
        <th>Weight</th>
        <th>Ai help</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="ticket in (tickets as Tickets)">
        <td>{{ ticket.title }}</td>
        <td>{{ ticket.description }}</td>
        <td>{{ ticket.creator }}</td>
        <td>{{ ticket.creationDate }}</td>
        <td>{{ ticket.weight }}</td>
        <td><button type="button" @click="showDialog(ticket.aiExplanation)">AI</button></td>
        <!-- <td>{{ ticket.aiExplanation }}</td> -->
      </tr>
    </tbody>
  </table>
  <dialog ref="aiDialog">
      <p>{{ dialogMessage  }}</p>
      <button @click="closeDialog">Chiudi</button>
    </dialog>
</template>

<style>
table {
  border: none;
  border-collapse: collapse;
}

th {
  font-size: x-large;
  font-weight: bold;
  border-bottom: solid .2rem hsla(160, 100%, 37%, 1);
}

th,
td {
  padding: 1rem;
}

td {
  text-align: center;
  font-size: large;
}

tr:nth-child(even) {
  background-color: var(--vt-c-text-dark-2);
}

button {
  padding: 10px 20px;
  font-size: 16px;
  cursor: pointer;
}
</style>