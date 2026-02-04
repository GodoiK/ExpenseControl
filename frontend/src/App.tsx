import React, { useEffect, useMemo, useState } from 'react'
import { api, API_BASE_URL, Category, FinancialRecord, SummaryTotals, User, UserSummaryItem } from './api'

const card: React.CSSProperties = {
  border: '1px solid #ddd',
  borderRadius: 12,
  padding: 16,
  marginBottom: 16,
}

type Page = 'people' | 'categories' | 'records' | 'summary'

function money(n: number) {
  return Number(n).toFixed(2)
}

function purposeLabel(p: number) {
  if (p === 0) return 'Despesa'
  if (p === 1) return 'Receita'
  if (p === 2) return 'Ambas'
  return String(p)
}

function recordTypeLabel(t: number) {
  return t === 1 ? 'Receita' : 'Despesa'
}

function isCategoryAllowed(purpose: number, recordType: number) {
  if (purpose === 2) return true
  if (recordType === 0) return purpose === 0
  return purpose === 1
}

export default function App() {
  const [page, setPage] = useState<Page>('people')
  const [error, setError] = useState<string>('')

  const [users, setUsers] = useState<User[]>([])
  const [categories, setCategories] = useState<Category[]>([])

  const [newUserName, setNewUserName] = useState('')
  const [newUserAge, setNewUserAge] = useState(18)
  const [editingUserId, setEditingUserId] = useState<string | null>(null)
  const [editUserName, setEditUserName] = useState('')
  const [editUserAge, setEditUserAge] = useState(18)

  const [newCatDesc, setNewCatDesc] = useState('')
  const [newCatPurpose, setNewCatPurpose] = useState(0)

  const [selectedUserId, setSelectedUserId] = useState<string>('')
  const [records, setRecords] = useState<FinancialRecord[]>([])
  const [recType, setRecType] = useState(0)
  const [recAmount, setRecAmount] = useState<number>(0)
  const [recDesc, setRecDesc] = useState('')
  const [recCategoryId, setRecCategoryId] = useState('')

  const [summaryItems, setSummaryItems] = useState<UserSummaryItem[]>([])
  const [summaryTotals, setSummaryTotals] = useState<SummaryTotals | null>(null)

  const selectedUser = useMemo(() => users.find(u => u.id === selectedUserId) ?? null, [users, selectedUserId])
  const allowedCategories = useMemo(
    () => categories.filter(c => isCategoryAllowed(c.purpose, recType)),
    [categories, recType],
  )

  const incomeRecords = useMemo(() => records.filter(r => r.type === 1), [records])
  const expenseRecords = useMemo(() => records.filter(r => r.type === 0), [records])
  const totalsForSelectedUser = useMemo(() => {
    const totalIncome = incomeRecords.reduce((acc, r) => acc + Number(r.amount), 0)
    const totalExpense = expenseRecords.reduce((acc, r) => acc + Number(r.amount), 0)
    return {
      totalIncome,
      totalExpense,
      balance: totalIncome - totalExpense,
    }
  }, [incomeRecords, expenseRecords])

  async function loadBase() {
    setError('')
    try {
      const [u, c] = await Promise.all([api.users.list(), api.categories.list()])
      setUsers(u)
      setCategories(c)

      if (!selectedUserId && u.length) setSelectedUserId(u[0].id)
    } catch (e: any) {
      setError(e?.message ?? String(e))
    }
  }

  async function loadRecords(userId: string) {
    if (!userId) return
    setError('')
    try {
      const r = await api.records.listByUser(userId)
      setRecords(r)
    } catch (e: any) {
      setError(e?.message ?? String(e))
    }
  }

  async function loadSummary() {
    setError('')
    try {
      const res = await api.users.summary()
      setSummaryItems(res.items)
      setSummaryTotals(res.totals)
    } catch (e: any) {
      setError(e?.message ?? String(e))
    }
  }

  useEffect(() => {
    loadBase()
  }, [])

  useEffect(() => {
    if (page === 'records' && selectedUserId) loadRecords(selectedUserId)
    if (page === 'summary') loadSummary()
  }, [page, selectedUserId])

  useEffect(() => {
    if (!allowedCategories.length) {
      setRecCategoryId('')
      return
    }
    if (!recCategoryId || !allowedCategories.some(c => c.id === recCategoryId)) {
      setRecCategoryId(allowedCategories[0].id)
    }
  }, [recType, categories])

  async function onCreateUser(e: React.FormEvent) {
    e.preventDefault()
    setError('')
    try {
      await api.users.create(newUserName, Number(newUserAge))
      setNewUserName('')
      await loadBase()
    } catch (err: any) {
      setError(err?.message ?? String(err))
    }
  }

  function startEditUser(u: User) {
    setEditingUserId(u.id)
    setEditUserName(u.name)
    setEditUserAge(u.age)
  }

  async function onSaveUser(e: React.FormEvent) {
    e.preventDefault()
    if (!editingUserId) return
    setError('')
    try {
      await api.users.update(editingUserId, editUserName, Number(editUserAge))
      setEditingUserId(null)
      await loadBase()
    } catch (err: any) {
      setError(err?.message ?? String(err))
    }
  }

  async function onDeleteUser(id: string) {
    if (!confirm('Excluir esta pessoa? Todas as transações dela serão apagadas.')) return
    setError('')
    try {
      await api.users.remove(id)
      if (selectedUserId === id) setSelectedUserId('')
      await loadBase()
      setRecords([])
    } catch (err: any) {
      setError(err?.message ?? String(err))
    }
  }

  async function onCreateCategory(e: React.FormEvent) {
    e.preventDefault()
    setError('')
    try {
      await api.categories.create(newCatDesc, Number(newCatPurpose))
      setNewCatDesc('')
      await loadBase()
    } catch (err: any) {
      setError(err?.message ?? String(err))
    }
  }

  async function onCreateRecord(e: React.FormEvent) {
    e.preventDefault()
    if (!selectedUserId) return
    setError('')
    try {
      await api.records.createForUser(selectedUserId, {
        categoryId: recCategoryId,
        type: Number(recType),
        amount: Number(recAmount),
        description: recDesc, 
      })
      setRecAmount(0)
      setRecDesc('')
      await loadRecords(selectedUserId)
    } catch (err: any) {
      setError(err?.message ?? String(err))
    }
  }

  return (
    <div style={{ maxWidth: 1100, margin: '0 auto', padding: 20, fontFamily: 'system-ui, Arial' }}>
      <h1 style={{ margin: 0 }}>ExpenseControl</h1>
      <div style={{ color: '#666', margin: '8px 0 18px' }}>
        
      </div>

      <div style={{ display: 'flex', gap: 8, flexWrap: 'wrap', marginBottom: 16 }}>
        <button onClick={() => setPage('people')} style={{ padding: '10px 14px', borderRadius: 10, border: '1px solid #ccc', background: page === 'people' ? '#111' : '#fff', color: page === 'people' ? '#fff' : '#111' }}>
          Pessoas
        </button>
        <button onClick={() => setPage('categories')} style={{ padding: '10px 14px', borderRadius: 10, border: '1px solid #ccc', background: page === 'categories' ? '#111' : '#fff', color: page === 'categories' ? '#fff' : '#111' }}>
          Categorias
        </button>
        <button onClick={() => setPage('records')} style={{ padding: '10px 14px', borderRadius: 10, border: '1px solid #ccc', background: page === 'records' ? '#111' : '#fff', color: page === 'records' ? '#fff' : '#111' }}>
          Lançamentos
        </button>
        <button onClick={() => setPage('summary')} style={{ padding: '10px 14px', borderRadius: 10, border: '1px solid #ccc', background: page === 'summary' ? '#111' : '#fff', color: page === 'summary' ? '#fff' : '#111' }}>
          Totais
        </button>
      </div>

      {error ? (
        <div style={{ ...card, borderColor: '#ffb3b3', background: '#fff5f5' }}>
          <b>Erro</b>
          <div style={{ whiteSpace: 'pre-wrap' }}>{error}</div>
        </div>
      ) : null}

      {page === 'people' ? (
        <div style={card}>
          <h2 style={{ marginTop: 0 }}>Cadastro de pessoas</h2>

          <form onSubmit={onCreateUser} style={{ display: 'flex', gap: 8, flexWrap: 'wrap', alignItems: 'center' }}>
            <input
              value={newUserName}
              onChange={e => setNewUserName(e.target.value)}
              placeholder="Nome"
              style={{ padding: 10, borderRadius: 8, border: '1px solid #ccc', minWidth: 260 }}
              maxLength={200}
              required
            />
            <input
              value={newUserAge}
              onChange={e => setNewUserAge(Number(e.target.value))}
              type="number"
              min={0}
              max={130}
              style={{ padding: 10, borderRadius: 8, border: '1px solid #ccc', width: 140 }}
              required
            />
            <button style={{ padding: '10px 14px', borderRadius: 8, border: '1px solid #333', background: '#111', color: '#fff' }}>
              Criar
            </button>
          </form>

          <div style={{ marginTop: 16, overflowX: 'auto' }}>
            <table style={{ width: '100%', borderCollapse: 'collapse' }}>
              <thead>
                <tr>
                  <th style={{ textAlign: 'left', padding: 8, borderBottom: '1px solid #eee' }}>Nome</th>
                  <th style={{ textAlign: 'right', padding: 8, borderBottom: '1px solid #eee' }}>Idade</th>
                  <th style={{ width: 220, padding: 8, borderBottom: '1px solid #eee' }}></th>
                </tr>
              </thead>
              <tbody>
                {users.map(u => (
                  <tr key={u.id}>
                    <td style={{ padding: 8, borderBottom: '1px solid #f2f2f2' }}>{u.name}</td>
                    <td style={{ padding: 8, borderBottom: '1px solid #f2f2f2', textAlign: 'right' }}>{u.age}</td>
                    <td style={{ padding: 8, borderBottom: '1px solid #f2f2f2', textAlign: 'right' }}>
                      <button onClick={() => startEditUser(u)} style={{ padding: '8px 10px', borderRadius: 8, border: '1px solid #ccc', marginRight: 8 }}>
                        Editar
                      </button>
                      <button onClick={() => onDeleteUser(u.id)} style={{ padding: '8px 10px', borderRadius: 8, border: '1px solid #ccc' }}>
                        Excluir
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          {editingUserId ? (
            <div style={{ marginTop: 16, paddingTop: 16, borderTop: '1px solid #eee' }}>
              <h3 style={{ marginTop: 0 }}>Editar pessoa</h3>
              <form onSubmit={onSaveUser} style={{ display: 'flex', gap: 8, flexWrap: 'wrap', alignItems: 'center' }}>
                <input
                  value={editUserName}
                  onChange={e => setEditUserName(e.target.value)}
                  placeholder="Nome"
                  style={{ padding: 10, borderRadius: 8, border: '1px solid #ccc', minWidth: 260 }}
                  maxLength={200}
                  required
                />
                <input
                  value={editUserAge}
                  onChange={e => setEditUserAge(Number(e.target.value))}
                  type="number"
                  min={0}
                  max={130}
                  style={{ padding: 10, borderRadius: 8, border: '1px solid #ccc', width: 140 }}
                  required
                />
                <button style={{ padding: '10px 14px', borderRadius: 8, border: '1px solid #333', background: '#111', color: '#fff' }}>
                  Salvar
                </button>
                <button type="button" onClick={() => setEditingUserId(null)} style={{ padding: '10px 14px', borderRadius: 8, border: '1px solid #ccc' }}>
                  Cancelar
                </button>
              </form>
            </div>
          ) : null}
        </div>
      ) : null}

      {page === 'categories' ? (
        <div style={card}>
          <h2 style={{ marginTop: 0 }}>Cadastro de categorias</h2>
          <form onSubmit={onCreateCategory} style={{ display: 'flex', gap: 8, flexWrap: 'wrap', alignItems: 'center' }}>
            <input
              value={newCatDesc}
              onChange={e => setNewCatDesc(e.target.value)}
              placeholder="Descrição"
              style={{ padding: 10, borderRadius: 8, border: '1px solid #ccc', minWidth: 320 }}
              maxLength={400}
              required
            />
            <select
              value={newCatPurpose}
              onChange={e => setNewCatPurpose(Number(e.target.value))}
              style={{ padding: 10, borderRadius: 8, border: '1px solid #ccc' }}
            >
              <option value={0}>Despesa</option>
              <option value={1}>Receita</option>
              <option value={2}>Ambas</option>
            </select>
            <button style={{ padding: '10px 14px', borderRadius: 8, border: '1px solid #333', background: '#111', color: '#fff' }}>
              Criar
            </button>
          </form>

          <div style={{ marginTop: 16, overflowX: 'auto' }}>
            <table style={{ width: '100%', borderCollapse: 'collapse' }}>
              <thead>
                <tr>
                  <th style={{ textAlign: 'left', padding: 8, borderBottom: '1px solid #eee' }}>Descrição</th>
                  <th style={{ textAlign: 'left', padding: 8, borderBottom: '1px solid #eee' }}>Finalidade</th>
                </tr>
              </thead>
              <tbody>
                {categories.map(c => (
                  <tr key={c.id}>
                    <td style={{ padding: 8, borderBottom: '1px solid #f2f2f2' }}>{c.description}</td>
                    <td style={{ padding: 8, borderBottom: '1px solid #f2f2f2' }}>{purposeLabel(c.purpose)}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      ) : null}

      {page === 'records' ? (
        <div style={card}>
          <h2 style={{ marginTop: 0 }}>Lançamentos</h2>

          <div style={{ marginBottom: 12, display: 'flex', gap: 8, alignItems: 'center', flexWrap: 'wrap' }}>
            <label><b>Pessoa:</b></label>
            <select
              value={selectedUserId}
              onChange={e => setSelectedUserId(e.target.value)}
              style={{ padding: 10, borderRadius: 8, border: '1px solid #ccc', minWidth: 320 }}
            >
              <option value="">-- selecione --</option>
              {users.map(u => (
                <option key={u.id} value={u.id}>{u.name} (idade {u.age})</option>
              ))}
            </select>
          </div>

          <div style={{ marginBottom: 8, color: '#666' }}>
            {selectedUser ? <>Pessoa selecionada: <b>{selectedUser.name}</b></> : 'Selecione uma pessoa para ver e registrar lançamentos.'}
          </div>

          <form onSubmit={onCreateRecord} style={{ display: 'flex', gap: 8, flexWrap: 'wrap', alignItems: 'center' }}>
            <select
              value={recType}
              onChange={e => setRecType(Number(e.target.value))}
              style={{ padding: 10, borderRadius: 8, border: '1px solid #ccc' }}
            >
              <option value={0}>Despesa</option>
              <option value={1}>Receita</option>
            </select>

            <select
              value={recCategoryId}
              onChange={e => setRecCategoryId(e.target.value)}
              style={{ padding: 10, borderRadius: 8, border: '1px solid #ccc', minWidth: 280 }}
              required
            >
              {allowedCategories.map(c => (
                <option key={c.id} value={c.id}>{c.description} ({purposeLabel(c.purpose)})</option>
              ))}
            </select>

            <input
              value={recAmount}
              onChange={e => setRecAmount(Number(e.target.value))}
              type="number"
              min={0}
              step="0.01"
              style={{ padding: 10, borderRadius: 8, border: '1px solid #ccc', width: 160 }}
              required
            />

            <input
              value={recDesc}
              onChange={e => setRecDesc(e.target.value)}
              placeholder="Descrição (opcional)"
              style={{ padding: 10, borderRadius: 8, border: '1px solid #ccc', minWidth: 320 }}
              maxLength={400}
            />

            <button disabled={!selectedUserId} style={{ padding: '10px 14px', borderRadius: 8, border: '1px solid #333', background: '#111', color: '#fff', opacity: selectedUserId ? 1 : 0.5 }}>
              Adicionar
            </button>
          </form>

          <div style={{ marginTop: 16, display: 'flex', gap: 12, flexWrap: 'wrap' }}>
            <div style={{ ...card, flex: '1 1 280px', marginBottom: 0 }}>
              <b>Total de receitas:</b> {money(totalsForSelectedUser.totalIncome)}
            </div>
            <div style={{ ...card, flex: '1 1 280px', marginBottom: 0 }}>
              <b>Total de despesas:</b> {money(totalsForSelectedUser.totalExpense)}
            </div>
            <div style={{ ...card, flex: '1 1 280px', marginBottom: 0 }}>
              <b>Saldo final:</b> {money(totalsForSelectedUser.balance)}
            </div>
          </div>

          <div style={{ marginTop: 16, display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 16 }}>
            <div style={{ ...card, marginBottom: 0, background: '#fff5f5', borderColor: '#ffd1d1' }}>
              <h3 style={{ marginTop: 0 }}>Despesas</h3>
              {expenseRecords.length === 0 ? (
                <div style={{ color: '#666' }}>Sem despesas.</div>
              ) : (
                <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                  <thead>
                    <tr>
                      <th style={{ textAlign: 'left', padding: 8, borderBottom: '1px solid #eee' }}>Data</th>
                      <th style={{ textAlign: 'left', padding: 8, borderBottom: '1px solid #eee' }}>Categoria</th>
                      <th style={{ textAlign: 'right', padding: 8, borderBottom: '1px solid #eee' }}>Valor</th>
                      <th style={{ textAlign: 'left', padding: 8, borderBottom: '1px solid #eee' }}>Descrição</th>
                    </tr>
                  </thead>
                  <tbody>
                    {expenseRecords.map(r => (
                      <tr key={r.id}>
                        <td style={{ padding: 8, borderBottom: '1px solid #f2f2f2' }}>{new Date(r.createdAt).toLocaleString()}</td>
                        <td style={{ padding: 8, borderBottom: '1px solid #f2f2f2' }}>{r.categoryDescription}</td>
                        <td style={{ padding: 8, borderBottom: '1px solid #f2f2f2', textAlign: 'right' }}>{money(r.amount)}</td>
                        <td style={{ padding: 8, borderBottom: '1px solid #f2f2f2' }}>{r.description || 'Sem descrição'}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              )}
            </div>

            <div style={{ ...card, marginBottom: 0, background: '#f4fff7', borderColor: '#c9f2d4' }}>
              <h3 style={{ marginTop: 0 }}>Receitas</h3>
              {incomeRecords.length === 0 ? (
                <div style={{ color: '#666' }}>Sem receitas.</div>
              ) : (
                <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                  <thead>
                    <tr>
                      <th style={{ textAlign: 'left', padding: 8, borderBottom: '1px solid #eee' }}>Data</th>
                      <th style={{ textAlign: 'left', padding: 8, borderBottom: '1px solid #eee' }}>Categoria</th>
                      <th style={{ textAlign: 'right', padding: 8, borderBottom: '1px solid #eee' }}>Valor</th>
                      <th style={{ textAlign: 'left', padding: 8, borderBottom: '1px solid #eee' }}>Descrição</th>
                    </tr>
                  </thead>
                  <tbody>
                    {incomeRecords.map(r => (
                      <tr key={r.id}>
                        <td style={{ padding: 8, borderBottom: '1px solid #f2f2f2' }}>{new Date(r.createdAt).toLocaleString()}</td>
                        <td style={{ padding: 8, borderBottom: '1px solid #f2f2f2' }}>{r.categoryDescription}</td>
                        <td style={{ padding: 8, borderBottom: '1px solid #f2f2f2', textAlign: 'right' }}>{money(r.amount)}</td>
                        <td style={{ padding: 8, borderBottom: '1px solid #f2f2f2' }}>{r.description || 'Sem descrição'}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              )}
            </div>
          </div>

          <div style={{ color: '#666', fontSize: 13, marginTop: 12 }}>
            O usuário final não digita GUID: ele seleciona a pessoa, e o front envia o ID automaticamente.
          </div>
        </div>
      ) : null}

      {page === 'summary' ? (
        <div style={card}>
          <h2 style={{ marginTop: 0 }}>Totais por pessoa</h2>

          <div style={{ marginBottom: 12, display: 'flex', gap: 8, flexWrap: 'wrap' }}>
            <button onClick={() => loadSummary()} style={{ padding: '10px 14px', borderRadius: 8, border: '1px solid #ccc' }}>
              Atualizar
            </button>
          </div>

          <div style={{ overflowX: 'auto' }}>
            <table style={{ width: '100%', borderCollapse: 'collapse' }}>
              <thead>
                <tr>
                  <th style={{ textAlign: 'left', padding: 8, borderBottom: '1px solid #eee' }}>Pessoa</th>
                  <th style={{ textAlign: 'right', padding: 8, borderBottom: '1px solid #eee' }}>Receitas</th>
                  <th style={{ textAlign: 'right', padding: 8, borderBottom: '1px solid #eee' }}>Despesas</th>
                  <th style={{ textAlign: 'right', padding: 8, borderBottom: '1px solid #eee' }}>Saldo</th>
                </tr>
              </thead>
              <tbody>
                {summaryItems.map(it => (
                  <tr key={it.id}>
                    <td style={{ padding: 8, borderBottom: '1px solid #f2f2f2' }}>{it.name}</td>
                    <td style={{ padding: 8, borderBottom: '1px solid #f2f2f2', textAlign: 'right' }}>{money(it.totalIncome)}</td>
                    <td style={{ padding: 8, borderBottom: '1px solid #f2f2f2', textAlign: 'right' }}>{money(it.totalExpense)}</td>
                    <td style={{ padding: 8, borderBottom: '1px solid #f2f2f2', textAlign: 'right' }}>{money(it.balance)}</td>
                  </tr>
                ))}
              </tbody>
              {summaryTotals ? (
                <tfoot>
                  <tr>
                    <td style={{ padding: 8, borderTop: '2px solid #ddd' }}><b>Total geral</b></td>
                    <td style={{ padding: 8, borderTop: '2px solid #ddd', textAlign: 'right' }}><b>{money(summaryTotals.totalIncome)}</b></td>
                    <td style={{ padding: 8, borderTop: '2px solid #ddd', textAlign: 'right' }}><b>{money(summaryTotals.totalExpense)}</b></td>
                    <td style={{ padding: 8, borderTop: '2px solid #ddd', textAlign: 'right' }}><b>{money(summaryTotals.balance)}</b></td>
                  </tr>
                </tfoot>
              ) : null}
            </table>
          </div>
        </div>
      ) : null}
    </div>
  )
}
