import React from 'react';
import styles from './index.css';
import * as microsoftTeams from "@microsoft/teams-js";
import { getContext, PrimaryButton, TeamsThemeContext, ThemeStyle } from 'msteams-ui-components-react';

class BasicLayout extends React.PureComponent {

    componentDidMount() {
        if (this.inTeams()) {
            microsoftTeams.initialize();
        }
    }

    render() {
        const context = getContext({
            baseFontSize: 16,
            style: ThemeStyle.Light
        });

        return (
            <TeamsThemeContext.Provider value={context}>
                <div className={styles.normal}>
                    <h1 className={styles.title}>Yay! Welcome to umi!</h1>
                    <PrimaryButton>button</PrimaryButton>
                    {this.props.children}
                </div>
            </TeamsThemeContext.Provider>
        );
    }

    private inTeams = () => {
        try {
            return window.self !== window.top;
        } catch (e) {
            return true;
        }
    }
};

export default BasicLayout;
