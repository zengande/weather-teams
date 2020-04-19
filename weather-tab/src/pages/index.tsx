import React, { useState } from 'react';
import styles from './index.css';
import * as microsoftTeams from '@microsoft/teams-js';
import * as jwt from 'jsonwebtoken';

export default function () {
    const [info, setinfo] = useState({})
    const [message, setMeessage] = useState("")
    microsoftTeams.getContext(context => {
        setinfo(context)

        microsoftTeams.authentication.getAuthToken({
            resources: ["api://52531682.ngrok.io/a08560e3-f1cb-4638-bdbd-c9ad8876417e"],
            successCallback: function (token: string) { 
                if(token!==message){
                    const result  = jwt.decode(token);
                    setMeessage(JSON.stringify(result))
                }
            },
            failureCallback: function (error: string) { error !== message && setMeessage("Failure: " + error); },
        });
    })




    return (
        <div className={styles.normal}>
            {JSON.stringify(info)}
            <div className={styles.welcome} />
            <h1>{message}</h1>
            <ul className={styles.list}>
                <li>To get started, edit <code>src/pages/index.js</code> and save to reload.</li>
                <li>
                    <a href="https://umijs.org/guide/getting-started.html">
                        Getting Started
          </a>
                </li>
            </ul>
        </div>
    );
}
