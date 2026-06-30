export default {
  start: () => {
    // 💡 绝对稳健的路径探测：直接看浏览器地址栏里有没有带仓库名 /CompassEx/
    const isGitHubPages = window.location.pathname.startsWith('/CompassEx/');
    const basePath = isGitHubPages ? '/CompassEx' : '';

    // 1. 引入 Pagefind 样式
    const link = document.createElement('link');
    link.href = `${basePath}/pagefind/pagefind-ui.css`;
    link.rel = 'stylesheet';
    document.head.appendChild(link);

    // 2. 注入绝对优先级的强力 CSS 样式（用 !important 压制所有默认样式）
    const style = document.createElement('style');
    style.innerHTML = `
      #pagefind-search-box {
        --pagefind-ui-scale: 0.85;
        width: 100% !important;
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
        box-shadow: 0 15px 40px rgba(0,0,0,0.15) !important;
        z-index: 99999 !important;
        padding: 25px !important;
        right: auto !important;
      }
      .pagefind-ui__drawer:empty {
        display: none !important;
      }
      mark {
        background-color: #fff3cd !important;
        color: #664d03 !important;
        padding: 1px 3px;
        border-radius: 3px;
        font-weight: bold;
      }
    `;
    document.head.appendChild(style);

    // 3. 异步加载 Pagefind 脚本并挂载
    const script = document.createElement('script');
    script.src = `${basePath}/pagefind/pagefind-ui.js`;
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
