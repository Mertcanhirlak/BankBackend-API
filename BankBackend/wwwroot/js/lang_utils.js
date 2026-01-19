// Language Utility for TR/EN Switching
// This script provides translation functionality for the frontend

const translations = {
    tr: {
        // Login Page
        'login.title': 'Bank Giriş',
        'login.subtitle': 'Hesabınıza erişmek için giriş yapın',
        'login.tc': 'T.C. Kimlik No',
        'login.password': 'Şifre',
        'login.button': 'Giriş Yap',
        'login.noAccount': 'Hesabınız yok mu?',
        'login.support': 'Müşteri Hizmetleri',
        'login.error.tcLength': 'T.C. Kimlik No 11 haneli olmalıdır.',
        'login.error.failed': 'Giriş başarısız! T.C. veya şifre hatalı.',
        'login.error.connection': 'Sunucuya bağlanılamadı.',

        // Admin Dashboard
        'admin.panel': 'Admin Panel',
        'admin.overview': 'Genel Bakış',
        'admin.customers': 'Müşteriler',
        'admin.riskDesk': 'Risk Masası',
        'admin.logout': 'Çıkış Yap',
        'admin.header': 'Yönetim Paneli',
        'admin.subtitle': 'Banka sistem durum özeti ve kontrolleri',

        // Stats Cards
        'stats.totalCustomers': 'Toplam Müşteri',
        'stats.totalDeposit': 'Toplam Mevduat',
        'stats.transactionVolume': 'İşlem Hacmi',
        'stats.riskEvents': 'Riskli Olay',
        'stats.systemActivity': 'Sistem Aktivitesi',

        // Customer Management
        'customer.management': 'Müşteri Yönetimi',
        'customer.search': 'Ad, TC veya ID (örn: 5)...',
        'customer.searchBtn': 'Ara',
        'customer.addNew': 'Yeni Ekle',
        'customer.id': 'ID',
        'customer.tc': 'TC Kimlik',
        'customer.name': 'Ad Soyad',
        'customer.role': 'Rol',
        'customer.registerDate': 'Kayıt Tarihi',
        'customer.actions': 'İşlemler',

        // Add Customer Modal
        'modal.addCustomer': 'Yeni Müşteri Ekle',
        'modal.tcNo': 'TC Kimlik No',
        'modal.firstName': 'Ad',
        'modal.lastName': 'Soyad',
        'modal.password': 'Şifre',
        'modal.passwordPlaceholder': 'Boş bırakılırsa TC olur',
        'modal.role': 'Rol',
        'modal.save': 'Kaydet',
        'modal.close': 'Kapat',

        // Customer Detail Modal
        'detail.title': 'Müşteri Yönetimi',
        'detail.userRole': 'Kullanıcı Rolü:',
        'detail.update': 'Güncelle',
        'detail.accounts': 'Hesaplar',
        'detail.history': 'İşlem Geçmişi',
        'detail.delete': 'Kullanıcıyı Sil',
        'detail.date': 'Tarih',
        'detail.description': 'Açıklama',
        'detail.amount': 'Tutar',
        'detail.deleteAccount': 'Sil',
        'detail.noAccount': 'Hesap bulunamadı.',

        // Risk Desk
        'risk.title': 'Şüpheli İşlem Logları',
        'risk.refresh': 'Yenile',
        'risk.accountId': 'Hesap ID',
        'risk.event': 'Olay Açıklaması',
        'risk.date': 'Tarih',

        // Alerts
        'alert.userAdded': 'Kullanıcı eklendi',
        'alert.error': 'Hata oluştu',
        'alert.roleUpdated': 'Rol güncellendi!',
        'alert.updateFailed': 'Güncelleme başarısız.',
        'alert.confirmRole': 'Bu kullanıcının yetkisini \'{role}\' olarak değiştirmek istiyor musunuz?',
        'alert.deleted': 'Silindi.',
        'alert.deleteFailed': 'Hata! Önce hesapları silin.',
        'alert.confirmDelete': 'Hesabı silmek istediğinize emin misiniz?',
        'alert.confirmDeleteUser': 'Kullanıcıyı silmek istediğinize emin misiniz?',
        'alert.unauthorized': 'YEtkİSİZ GİRİŞ!\nRolünüz: {role}\nGiriş yetkisi: ADMIN veya PATRON',

        // Common
        'common.loading': 'Yükleniyor...', 
        'common.noData': 'İşlem yok.',

        // User Dashboard
        'dash.welcome': 'Hoşgeldiniz',
        'dash.customer': 'Müşteri',
        'dash.myAccounts': 'Hesaplarım',
        'dash.newAccount': 'Yeni Hesap Aç',
        'dash.accountNo': 'Hesap No',
        'dash.balance': 'Bakiye',
        'dash.transaction': 'İşlem',
        'dash.noAccounts': 'Henüz bir hesabınız bulunmuyor.',
        'dash.accountsLoading': 'Hesap bilgileri yüklenemedi.',

        // AI Assistant
        'ai.title': 'Akıllı Finans Asistanı',
        'ai.subtitle': 'Harcamalarınızı analiz edip size özel tasarruf tavsiyeleri alabilirsiniz.',
        'ai.analyze': 'Analiz Et',
        'ai.reanalyze': 'Yeniden Analiz Et',
        'ai.thinking': 'Düşünüyor...', 
        'ai.resultTitle': 'Analiz Sonucu',
        'ai.recommendations': 'Tavsiyeler:',
        'ai.noRecommendations': 'Tavsiye bulunamadı.',
        'ai.error': 'Asistan şu an yoğun, lütfen sonra tekrar deneyin.',

        // Transactions
        'trans.recent': 'Son İşlemler',
        'trans.date': 'Tarih',
        'trans.description': 'Açıklama',
        'trans.amount': 'Tutar',
        'trans.status': 'Durum',
        'trans.loading': 'İşlem geçmişi yükleniyor...', 
        'trans.noHistory': 'Henüz işlem geçmişiniz yok.',
        'trans.income': 'Giriş',
        'trans.expense': 'Çıkış',
        'trans.transfer': 'Transfer',
        'trans.atmDeposit': 'ATM Para Yatırma',
        'trans.atmWithdraw': 'ATM Para Çekme',
        'trans.toSelf': 'Kendi Hesabına',
        'trans.sentTo': 'Alıcı:',

        // Chart
        'chart.title': 'Finansal Özet',
        'chart.subtitle': 'Son işlemlere göre dağılım',
        'chart.income': 'Gelir',
        'chart.expense': 'Gider',
        'chart.noData': 'Veri Yok',
        'chart.noTransactions': 'Henüz işlem yok',

        // Transfer Modal
        'transfer.title': 'Para Transferi',
        'transfer.sender': 'Gönderen Hesap',
        'transfer.receiver': 'Alıcı Hesap ID',
        'transfer.receiverPlaceholder': 'Örn: 102',
        'transfer.amount': 'Tutar (TL)',
        'transfer.description': 'Açıklama',
        'transfer.descPlaceholder': 'Kira ödemesi vb.',
        'transfer.cancel': 'İptal',
        'transfer.submit': 'Transfer Yap',
        'transfer.success': 'Transfer başarılı!',
        'transfer.fillFields': 'Alanları doldurun',
        'transfer.noAccounts': 'Hesabınız yok!',

        // ATM Modal
        'atm.title': 'ATM İşlemi',
        'atm.deposit': 'Para Yatır',
        'atm.withdraw': 'Para Çek',
        'atm.amount': 'Tutar',
        'atm.confirm': 'İşlemi Onayla',
        'atm.success': 'İşlem Başarılı!',
        'atm.invalidAmount': 'Geçersiz tutar',

        // Account Actions
        'account.confirmNew': 'Yeni bir vadesiz hesap açmak istiyor musunuz?',
        'account.created': 'Hesap açıldı!',
        'account.error': 'Hata!',

        // Session
        'session.expired': 'Oturum süreniz dolmuş veya geçersiz giriş. Lütfen tekrar giriş yapın.',

        // Roles
        'role.admin': 'Yönetici',
        'role.yonetici': 'Yönetici',
        'role.musteri': 'Müşteri',
        'role.patron': 'Patron',

        'trans.receivedFrom': 'Gelen:',

        // Risk Logs
        'risk.withdrawal': 'Hesaptan para çıkışı oldu. Kontrol edilebilir.',
        'risk.highTransfer': 'Yüksek Tutar Transferi:',
        'admin.actions': 'Aksiyon',
        'risk.highAmount': 'Yüksek Tutar Transferi'
    },
    en: {
        // Login Page
        'login.title': 'Bank Login',
        'login.subtitle': 'Sign in to access your account',
        'login.tc': 'ID Number',
        'login.password': 'Password',
        'login.button': 'Sign In',
        'login.noAccount': 'Don\'t have an account?',
        'login.support': 'Customer Support',
        'login.error.tcLength': 'ID Number must be 11 digits.',
        'login.error.failed': 'Login failed! Invalid ID or password.',
        'login.error.connection': 'Could not connect to server.',

        // Admin Dashboard
        'admin.panel': 'Admin Panel',
        'admin.overview': 'Overview',
        'admin.customers': 'Customers',
        'admin.riskDesk': 'Risk Desk',
        'admin.logout': 'Logout',
        'admin.header': 'Management Panel',
        'admin.subtitle': 'Bank system status summary and controls',

        // Stats Cards
        'stats.totalCustomers': 'Total Customers',
        'stats.totalDeposit': 'Total Deposit',
        'stats.transactionVolume': 'Transaction Volume',
        'stats.riskEvents': 'Risk Events',
        'stats.systemActivity': 'System Activity',

        // Customer Management
        'customer.management': 'Customer Management',
        'customer.search': 'Name, ID or Number (e.g: 5)...',
        'customer.searchBtn': 'Search',
        'customer.addNew': 'Add New',
        'customer.id': 'ID',
        'customer.tc': 'ID Number',
        'customer.name': 'Full Name',
        'customer.role': 'Role',
        'customer.registerDate': 'Register Date',
        'customer.actions': 'Actions',

        // Add Customer Modal
        'modal.addCustomer': 'Add New Customer',
        'modal.tcNo': 'ID Number',
        'modal.firstName': 'First Name',
        'modal.lastName': 'Last Name',
        'modal.password': 'Password',
        'modal.passwordPlaceholder': 'Leave empty to use ID',
        'modal.role': 'Role',
        'modal.save': 'Save',
        'modal.close': 'Close',

        // Customer Detail Modal
        'detail.title': 'Customer Management',
        'detail.userRole': 'User Role:',
        'detail.update': 'Update',
        'detail.accounts': 'Accounts',
        'detail.history': 'Transaction History',
        'detail.delete': 'Delete User',
        'detail.date': 'Date',
        'detail.description': 'Description',
        'detail.amount': 'Amount',
        'detail.deleteAccount': 'Delete',
        'detail.noAccount': 'No accounts found.',

        // Risk Desk
        'risk.title': 'Suspicious Activity Logs',
        'risk.refresh': 'Refresh',
        'risk.accountId': 'Account ID',
        'risk.event': 'Event Description',
        'risk.date': 'Date',

        // Alerts
        'alert.userAdded': 'User added successfully',
        'alert.error': 'An error occurred',
        'alert.roleUpdated': 'Role updated successfully!',
        'alert.updateFailed': 'Update failed.',
        'alert.confirmRole': 'Do you want to change this user\'s role to \'{role}\'?',
        'alert.deleted': 'Deleted successfully.',
        'alert.deleteFailed': 'Error! Delete accounts first.',
        'alert.confirmDelete': 'Are you sure you want to delete this account?',
        'alert.confirmDeleteUser': 'Are you sure you want to delete this user?',
        'alert.unauthorized': 'UNAUTHORIZED ACCESS!\nYour role: {role}\nRequired: ADMIN or PATRON',

        // Common
        'common.loading': 'Loading...', 
        'common.noData': 'No transactions.',

        // User Dashboard
        'dash.welcome': 'Welcome',
        'dash.customer': 'Customer',
        'dash.myAccounts': 'My Accounts',
        'dash.newAccount': 'Open New Account',
        'dash.accountNo': 'Account No',
        'dash.balance': 'Balance',
        'dash.transaction': 'Transaction',
        'dash.noAccounts': 'You don\'t have any accounts yet.',
        'dash.accountsLoading': 'Failed to load account information.',
        'ai.title': 'Smart Finance Assistant',
        'ai.subtitle': 'Get personalized savings advice by analyzing your spending.',
        'ai.analyze': 'Analyze',
        'ai.reanalyze': 'Analyze Again',
        'ai.thinking': 'Thinking...', 
        'ai.resultTitle': 'Analysis Result',
        'ai.recommendations': 'Recommendations:',
        'ai.noRecommendations': 'No recommendations found.',
        'ai.error': 'Assistant is busy, please try again later.',
        'trans.recent': 'Recent Transactions',
        'trans.date': 'Date',
        'trans.description': 'Description',
        'trans.amount': 'Amount',
        'trans.status': 'Status',
        'trans.loading': 'Loading transaction history...', 
        'trans.noHistory': 'No transaction history yet.',
        'trans.income': 'Income',
        'trans.expense': 'Expense',
        'trans.transfer': 'Transfer',
        'trans.atmDeposit': 'ATM Deposit',
        'trans.atmWithdraw': 'ATM Withdrawal',
        'trans.toSelf': 'To Own Account',
        'trans.sentTo': 'Sent to',

        // Roles
        'role.admin': 'Manager',
        'role.yonetici': 'Manager',
        'role.musteri': 'Customer',
        'role.patron': 'Executive',

        'trans.receivedFrom': 'Received from:',

        // Risk Logs
        'risk.withdrawal': 'Account withdrawal occurred. Can be reviewed.',
        'risk.highTransfer': 'High Amount Transfer:',

        'chart.title': 'Financial Summary',
        'chart.subtitle': 'Distribution based on recent transactions',
        'chart.income': 'Income',
        'chart.expense': 'Expense',
        'chart.noData': 'No Data',
        'chart.noTransactions': 'No transactions yet',
        'transfer.title': 'Money Transfer',
        'transfer.sender': 'Sender Account',
        'transfer.receiver': 'Receiver Account ID',
        'transfer.receiverPlaceholder': 'e.g: 102',
        'transfer.amount': 'Amount (TL)',
        'transfer.description': 'Description',
        'transfer.descPlaceholder': 'Rent payment etc.',
        'transfer.cancel': 'Cancel',
        'transfer.submit': 'Transfer',
        'transfer.success': 'Transfer successful!',
        'transfer.fillFields': 'Please fill all fields',
        'transfer.noAccounts': 'You have no accounts!',
        'atm.title': 'ATM Transaction',
        'atm.deposit': 'Deposit',
        'atm.withdraw': 'Withdraw',
        'atm.amount': 'Amount',
        'atm.confirm': 'Confirm Transaction',
        'atm.success': 'Transaction Successful!',
        'atm.invalidAmount': 'Invalid amount',
        'account.confirmNew': 'Do you want to open a new demand deposit account?',
        'account.created': 'Account created!',
        'account.error': 'Error!',
        'session.expired': 'Your session has expired or invalid login. Please sign in again.',
        'admin.actions': 'Actions',
        'risk.highAmount': 'High Amount Transfer'
    }
};

// Get current language from localStorage or default to Turkish
function getCurrentLang() {
    return localStorage.getItem('lang') || 'tr';
}

// Set language
function setLang(lang) {
    localStorage.setItem('lang', lang);
    applyTranslations();
    window.dispatchEvent(new Event('lang-change'));
}

// Get translation for a key
function t(key) {
    const lang = getCurrentLang();
    return translations[lang][key] || key;
}

// Apply translations to all elements with data-lang attribute
function applyTranslations() {
    const lang = getCurrentLang();
    document.querySelectorAll('[data-lang]').forEach(el => {
        const key = el.getAttribute('data-lang');
        const translation = translations[lang][key];

        if (translation) {
            // Check if it's an input placeholder
            if (el.tagName === 'INPUT' && el.hasAttribute('placeholder')) {
                el.placeholder = translation;
            } else {
                el.textContent = translation;
            }
        }
    });

    // Update language toggle buttons
    document.querySelectorAll('.lang-btn').forEach(btn => {
        btn.classList.remove('active');
    });
    document.querySelector(`.lang-btn[data-lang-code="${lang}"]`)?.classList.add('active');
}

// Helper to localize transaction descriptions
function localizeTransaction(trxDesc) {
    if (!trxDesc) return t('trans.transfer');

    let d = trxDesc.trim();

    // Exact matches
    if (d === 'Para Transferi') return t('trans.transfer');
    if (d === 'ATM Para Yatırma') return t('trans.atmDeposit');
    if (d === 'ATM Para Çekme') return t('trans.atmWithdraw');

    // Risk-specific exact matches
    if (d === 'Hesaptan para çıkışı oldu. Kontrol edilebilir.') return t('risk.withdrawal');
    
    // Check for NEW English pattern
    if (d.startsWith('HIGH_AMOUNT_TRANSFER:')) {
        return d.replace('HIGH_AMOUNT_TRANSFER:', t('risk.highAmount') + ':');
    }

    // Check for OLD Turkish pattern (for backward compatibility)
    if (d.startsWith('Yüksek Tutar Transferi:')) return d.replace('Yüksek Tutar Transferi:', t('risk.highTransfer'));

    // Partial matches
    if (d.startsWith('Giden ->')) return d.replace('Giden ->', t('trans.sentTo') + ' ');
    if (d.includes('Sent to')) return d.replace('Sent to', t('trans.sentTo') + ' ');

    // Incoming
    if (d.startsWith('Gelen <-')) return d.replace('Gelen <-', t('trans.receivedFrom') + ' ');
    if (d.includes('Received from')) return d.replace('Received from', t('trans.receivedFrom') + ' ');

    return d;
}

// Initialize on page load
document.addEventListener('DOMContentLoaded', () => {
    applyTranslations();
});