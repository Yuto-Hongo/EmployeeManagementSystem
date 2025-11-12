import axios from   "axios";

// APIのベースURL
const api = axios.create({
    baseURL: "http://localhost:5211/api",
    headers: {
        "Content-Type": "application/json"
    }
});

// リクエストごとにJWTトークンを自動付与(ログイン後に使用)
api.interceptors.request.use(config => {
    const token = localStorage.getItem("token"); //保存済トークン取得
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});
    
export default api;