export default {
  start: () => {
    const isGitHubPages = window.location.pathname.startsWith('/CompassEx/');
    const basePath = isGitHubPages ? '/CompassEx' : '';
    
    // 💡 核心防缓存机制：通过生成一个随机版本号，欺骗并强制浏览器必须重新下载新文件
    const versionToken = `v=${new Date().getTime()}`;

    // 1. 引入 Pagefind 样式（加上随机后缀）
    const link = document.createElement('link');
    link.href = `${basePath}/pagefind/pagefind-ui.css?${versionToken}`; // 👈 加了后缀
    link.rel = 'stylesheet';
    document.head.appendChild(link);

    // 2. 注入绝对优先级的强力 CSS 样式
    const style = document.createElement('style');
    style.innerHTML = `
      #pagefind-search-box {
        --pagefind-ui-scale: 0.85;
        width: 100% !important;
        --pagefind-ui-background: var(--bs-body-bg, #ffffff) !important;
        --pagefind-ui-text: var(--bs-body-color, #212529) !important;
        --pagefind-ui-primary: var(--bs-primary, #0d6efd) !important;
        --pagefind-ui-border: var(--bs-border-color, #dee2e6) !important;
      }
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
        color: var(--bs-body-color, #212529) !important;
      }
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

    // 3. 异步加载 Pagefind 脚本（加上随机后缀）
    const script = document.createElement('script');
    script.src = `${basePath}/pagefind/pagefind-ui.js?${versionToken}`; // 👈 加了后缀
    script.onload = () => {
      const navContainer = document.querySelector('.navbar-nav') || document.querySelector('nav');
      
      if (navContainer) {
        const wrapper = document.createElement('div');
        wrapper.style.cssText = 'width: 220px; margin-left: auto; margin-right: 15px; display: flex; align-items: center; position: relative;';
        
        const customSearchContainer = document.createElement('div');
        customSearchContainer.id = 'pagefind-search-box';
        
        wrapper.appendChild(customSearchContainer);
        navContainer.appendChild(wrapper);

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
