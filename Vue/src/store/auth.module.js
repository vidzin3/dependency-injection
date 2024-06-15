import api from "@/utils/utilities";
import { defineStore } from "pinia";


export const useAute = defineStore('auth',{
    state:() => ({
        _authentication:false,
        _accessToken:null,
        _refreshToken:null,
        _isrefreshToken:false
    }),
    getters:{
        authenticated() {
            return this._authentication;
        },
        accessToken() {
            return this._accessToken;
        }
    },
    actions:{
        login(payload){
            api
                .post('')
                .then((res) => {
                    this._accessToken = res.data.token
                    this._authentication = true
                })
        },
        refreshToken() {
            return new Promise((resolve, reject) => {
              this._isTokenRefreshing = true;
      
              api
                .post("/refresh", {
                  refresh_token: this._refreshToken,
                  client_id: import.meta.env.VITE_CLIENT_ID,
                  client_secret: import.meta.env.VITE_CLIENT_SECRET,
                })
                .then((response) => {
                  if (response.data.data) {
                    this._authenticated = true;
                  }
      
                  resolve();
                })
                .catch((response) => {
                  this._authenticated = false;
      
                  localStorage.setItem(
                    "redirect",
                    window.location
                      .toString()
                      .replace('https://localhost' + "/", "")
                  );
      
                  reject(response);
                })
                .finally(() => {
                  this._isrefreshToken = false;
                });
            });
          },
    },
    persist:{
        storage:localStorage
    }
})