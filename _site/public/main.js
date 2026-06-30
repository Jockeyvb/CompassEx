export default {
  start: () => {
    // 💡 绝对稳健的路径探测：直接看浏览器地址栏里有没有带仓库名 /CompassEx/
    const isGitHubPages = window.location.pathname.startsWith('/CompassEx/');
    const basePath = isGitHubPages ? '/CompassEx' : '';
    
    // 💡 核心防缓存机制
    const versionToken = `v=${new Date().getTime()}`;

    // 1. 引入 Pagefind 样式
    const link = document.createElement('link');
    link.href = `${basePath}/pagefind/pagefind-ui.css?${versionToken}`;
    link.rel = 'stylesheet';
    document.head.appendChild(link);

    // 2. 注入绝对优先级的强力 CSS 样式（彻底解决本地和线上的浅色/深色文字问题）
    const style = document.createElement('style');
    style.innerHTML = `
      #pagefind-search-box {
        --pagefind-ui-scale: 0.85;
        width: 100% !important;
      }
      
      /* ==================== 🛠️ 核心色彩修正开始 ==================== */
      
      /* ☀️ 默认状态下（浅色模式）：强制输入框文字和图标为深灰色/黑色 */
      #pagefind-search-box .pagefind-ui__search-input,
      .pagefind-ui__search-input {
        color: #212529 !important;
        background-color: #ffffff !important;
      }
      #pagefind-search-box .pagefind-ui__search-clear,
      .pagefind-ui__search-clear {
        color: #212529 !important;
      }
      
      /* 🌙 当 DocFX 切换到深色模式时：强制输入框文字和清除按钮变成纯白色 */
      html[data-bs-theme="dark"] #pagefind-search-box .pagefind-ui__search-input,
      html[data-bs-theme="dark"] .pagefind-ui__search-input {
        color: #ffffff !important;
        background-color: #2b3035 !important; /* 匹配 Bootstrap 5 的深色输入框背景 */
      }
      html[data-bs-theme="dark"] #pagefind-search-box .pagefind-ui__search-clear,
      html[data-bs-theme="dark"] .pagefind-ui__search-clear {
        color: #ffffff !important;
      }
      
      /* ==================== 🛠️ 核心色彩修正结束 ==================== */

      /* 强制将结果面板从右侧剥离，变成全屏居中 800px 大弹窗 */
      #pagefind-search-box .pagefind-ui__drawer,
      .pagefind-ui__drawer {
        position: fixed !important;
        top: 75px !important;
        left: 50% !important;
        transform: translateX(-50%) !important;
        width: 800px !important;
        max-width: 95vw !important;
        max-height: 75vh !important;
        overflow-y: auto !important;
        background: var(--bs-body-bg, #ffffff) !important;
        border: 1px solid var(--bs-border-color, #e0e0e0) !important;
        border-radius: 12px !important;
        box-shadow: 0 15px 40px rgba(0,0,0,0.25) !important;
        z-index: 99999 !important;
        padding: 25px !important;
        right: auto !important;
      }
      
      /* 弹窗内部文字颜色跟随系统自适应 */
      .pagefind-ui__result-title, .pagefind-ui__result-title a {
        color: var(--bs-body-color, #212529) !important;
      }
      .pagefind-ui__result-excerpt {
        color: var(--bs-secondary-color, #6c757d) !important;
      }
      
      .pagefind-ui__drawer:empty {
        display: none !important;
      }
      mark {
        background-color: var(--bs-warning-border-subtle, #fefe10) !important;
        color: var(--bs-warning-text-emphasis, #664d03) !important;
        padding: 1px 3px;
        border-radius: 3px;
        font-weight: bold;
      }
    `;
    document.head.appendChild(style);

    // 3. 异步加载 Pagefind 脚本并挂载
    const script = document.createElement('script');
    script.src = `${basePath}/pagefind/pagefind-ui.js?${versionToken}`;
    script.onload = () => {
      const navContainer = document.querySelector('.navbar-nav') || document.querySelector('nav');
      
      if (navContainer) {
        const wrapper = document.createElement('div');
        wrapper.style.cssText = 'width: 220px; margin-left: auto; margin-right: 15px; display: flex; align-items: center; position: relative;';
        
        const customSearchContainer = document.createElement('div');
        customSearchContainer.id = 'pagefind-search-box';
        
        wrapper.appendChild(customSearchContainer);
        navContainer.appendChild(wrapper);

        // 初始化 Pagefind
        new PagefindUI({ 
          element: "#pagefind-search-box", 
          bundlePath: `${basePath}/pagefind/`,
          showSubResults: true,
          resetStyles: false 
        });
      }
    };
    document.head.appendChild(script);
  }
}
