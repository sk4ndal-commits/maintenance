<template>
  <div class="users-view">
    <div class="view-header">
      <h1>Benutzerverwaltung</h1>
      <button class="btn-primary" @click="openCreateModal">+ Benutzer anlegen</button>
    </div>

    <div v-if="error" class="error-banner">{{ error }}</div>

    <table class="data-table">
      <thead>
        <tr>
          <th>Name</th>
          <th>E-Mail</th>
          <th>Rolle</th>
          <th>Status</th>
          <th>Aktionen</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="user in users" :key="user.technicianId">
          <td>{{ user.name }}</td>
          <td>{{ user.email }}</td>
          <td><span class="badge badge-role">{{ user.role }}</span></td>
          <td>
            <span :class="['badge', user.isActive ? 'badge-success' : 'badge-danger']">
              {{ user.isActive ? 'Aktiv' : 'Inaktiv' }}
            </span>
          </td>
          <td class="actions">
            <button class="btn-secondary" @click="openEditModal(user)">Bearbeiten</button>
            <button
              :class="user.isActive ? 'btn-danger' : 'btn-success'"
              @click="toggleActive(user)"
            >
              {{ user.isActive ? 'Deaktivieren' : 'Aktivieren' }}
            </button>
          </td>
        </tr>
        <tr v-if="users.length === 0">
          <td colspan="5" class="empty-state">Keine Benutzer vorhanden.</td>
        </tr>
      </tbody>
    </table>

    <!-- Create / Edit Modal -->
    <div v-if="showModal" class="modal-overlay" @click.self="closeModal">
      <div class="modal">
        <h2>{{ editingUser ? 'Benutzer bearbeiten' : 'Neuen Benutzer anlegen' }}</h2>
        <form @submit.prevent="handleSubmit" class="modal-form">
          <div class="form-group">
            <label>Name</label>
            <input v-model="form.name" type="text" required placeholder="Vollständiger Name" />
          </div>
          <div class="form-group">
            <label>E-Mail</label>
            <input v-model="form.email" type="email" required placeholder="name@example.com" />
          </div>
          <div v-if="!editingUser" class="form-group">
            <label>Passwort</label>
            <input v-model="form.password" type="password" required placeholder="••••••••" />
          </div>
          <div class="form-group">
            <label>Rolle</label>
            <select v-model="form.role" required>
              <option value="Admin">Admin</option>
              <option value="Planner">Planner</option>
              <option value="Technician">Technician</option>
            </select>
          </div>
          <div v-if="editingUser" class="form-group">
            <button type="button" class="btn-secondary" @click="showResetSection = !showResetSection">
              {{ showResetSection ? 'Passwort zurücksetzen ausblenden' : 'Passwort zurücksetzen' }}
            </button>
          </div>
          <div v-if="showResetSection" class="form-group">
            <label>Neues Passwort</label>
            <input v-model="newPassword" type="password" placeholder="••••••••" />
          </div>
          <p v-if="formError" class="error-message">{{ formError }}</p>
          <div class="modal-actions">
            <button type="button" class="btn-secondary" @click="closeModal">Abbrechen</button>
            <button type="submit" class="btn-primary" :disabled="saving">
              {{ saving ? 'Speichern...' : 'Speichern' }}
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { userApi } from '../api/userApi'
import type { User } from '../types/asset'

const users = ref<User[]>([])
const error = ref('')
const showModal = ref(false)
const editingUser = ref<User | null>(null)
const saving = ref(false)
const formError = ref('')
const showResetSection = ref(false)
const newPassword = ref('')

const form = ref({ name: '', email: '', password: '', role: 'Technician' })

async function loadUsers() {
  try {
    users.value = await userApi.getAll()
  } catch {
    error.value = 'Benutzer konnten nicht geladen werden.'
  }
}

function openCreateModal() {
  editingUser.value = null
  form.value = { name: '', email: '', password: '', role: 'Technician' }
  formError.value = ''
  showModal.value = true
}

function openEditModal(user: User) {
  editingUser.value = user
  form.value = { name: user.name, email: user.email, password: '', role: user.role }
  formError.value = ''
  showResetSection.value = false
  newPassword.value = ''
  showModal.value = true
}

function closeModal() {
  showModal.value = false
}

async function handleSubmit() {
  formError.value = ''
  saving.value = true
  try {
    if (editingUser.value) {
      await userApi.update(editingUser.value.technicianId, {
        name: form.value.name,
        email: form.value.email,
        role: form.value.role,
      })
      if (showResetSection.value && newPassword.value) {
        await userApi.resetPassword(editingUser.value.technicianId, newPassword.value)
      }
    } else {
      await userApi.create({
        name: form.value.name,
        email: form.value.email,
        password: form.value.password,
        role: form.value.role,
      })
    }
    closeModal()
    await loadUsers()
  } catch {
    formError.value = 'Fehler beim Speichern. Bitte erneut versuchen.'
  } finally {
    saving.value = false
  }
}

async function toggleActive(user: User) {
  try {
    await userApi.setActive(user.technicianId, !user.isActive)
    await loadUsers()
  } catch {
    error.value = 'Status konnte nicht geändert werden.'
  }
}

onMounted(loadUsers)
</script>

<style scoped>
.users-view {
  padding: 32px;
}

.view-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 24px;
}

.view-header h1 {
  font-size: 1.5rem;
  font-weight: 700;
  margin: 0;
}

.error-banner {
  background: #fee2e2;
  color: #dc2626;
  padding: 12px 16px;
  margin-bottom: 16px;
  font-size: 0.875rem;
}

.data-table {
  width: 100%;
  border-collapse: collapse;
  background: #fff;
  box-shadow: 0 1px 4px rgba(0, 0, 0, 0.08);
}

.data-table th,
.data-table td {
  padding: 12px 16px;
  text-align: left;
  border-bottom: 1px solid #f0f0f0;
  font-size: 0.9rem;
}

.data-table th {
  font-weight: 600;
  background: #fafafa;
  color: #555;
}

.empty-state {
  text-align: center;
  color: #999;
  padding: 32px;
}

.actions {
  display: flex;
  gap: 8px;
}

.badge {
  display: inline-block;
  padding: 2px 10px;
  border-radius: 12px;
  font-size: 0.78rem;
  font-weight: 600;
}

.badge-role {
  background: #e0e7ff;
  color: #3730a3;
}

.badge-success {
  background: #dcfce7;
  color: #16a34a;
}

.badge-danger {
  background: #fee2e2;
  color: #dc2626;
}

.btn-primary {
  padding: 8px 16px;
  background: #2563eb;
  color: #fff;
  border: none;
  font-size: 0.9rem;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.2s;
}

.btn-primary:hover:not(:disabled) {
  background: #1d4ed8;
}

.btn-primary:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-secondary {
  padding: 6px 12px;
  background: #f3f4f6;
  color: #374151;
  border: 1px solid #d1d5db;
  font-size: 0.85rem;
  cursor: pointer;
  transition: background 0.2s;
}

.btn-secondary:hover {
  background: #e5e7eb;
}

.btn-danger {
  padding: 6px 12px;
  background: #fee2e2;
  color: #dc2626;
  border: 1px solid #fca5a5;
  font-size: 0.85rem;
  cursor: pointer;
  transition: background 0.2s;
}

.btn-danger:hover {
  background: #fecaca;
}

.btn-success {
  padding: 6px 12px;
  background: #dcfce7;
  color: #16a34a;
  border: 1px solid #86efac;
  font-size: 0.85rem;
  cursor: pointer;
  transition: background 0.2s;
}

.btn-success:hover {
  background: #bbf7d0;
}

.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.4);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 100;
}

.modal {
  background: #fff;
  padding: 32px;
  min-width: 400px;
  max-width: 520px;
  width: 100%;
  box-shadow: 0 4px 24px rgba(0, 0, 0, 0.15);
}

.modal h2 {
  margin: 0 0 24px;
  font-size: 1.2rem;
  font-weight: 700;
}

.modal-form {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.form-group label {
  font-size: 0.875rem;
  font-weight: 500;
}

.form-group input,
.form-group select {
  padding: 10px 12px;
  border: 1px solid #ddd;
  font-size: 0.95rem;
  outline: none;
  transition: border-color 0.2s;
}

.form-group input:focus,
.form-group select:focus {
  border-color: #2563eb;
}

.error-message {
  color: #dc2626;
  font-size: 0.875rem;
  margin: 0;
}

.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 8px;
  margin-top: 8px;
}
</style>
